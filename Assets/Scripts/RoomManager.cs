using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<RoomSpawner> spawns = new List<RoomSpawner>();
    public List<RoomVolume> spaceOccupied = new List<RoomVolume>();
    public List<RoomAdjacencyChecker> adjacencyCheckers = new List<RoomAdjacencyChecker>();
    public List<RoomSpawner> obstructedSpawns = new List<RoomSpawner>();
    public RoomSpawner.DoorOpening directionBuiltFrom;
    public List<RoomSpawner.DoorOpening> requirements = new List<RoomSpawner.DoorOpening>();
    public List<RoomManager> connectedRooms = new List<RoomManager>();
    public List<DoorBehaviour> connectedDoors = new List<DoorBehaviour>();

    private FloorManager roomBank;

    [HideInInspector] public GameObject renderable;

    private const float ROOM_GENERATION_DELAY = 0.25f;
    private const float ROOM_COMPATIBILITY_DELAY = 0.1f;


    // Waits half a second then creates the necessary rooms.
    private void Start()
    {
        // Setup the roombank then wait for a bit so our spawn boxes can register their collisions so we can evaluate them.
        roomBank = GameObject.Find("FloorManager").GetComponent<FloorManager>();

        foreach (RoomAdjacencyChecker adjacencyChecker in adjacencyCheckers)
            adjacencyChecker.gameObject.SetActive(false);

        Invoke("CheckRoomCompatibility", ROOM_COMPATIBILITY_DELAY);
        StartCoroutine("CheckRoomCompatibility");
    }

    // Used to create the dungeon layout.
    public void GenerateDungeon()
    {
        //Debug.Log("WE ARE GENERATING THE DUNGEON FOR THIS ROOM : " + gameObject.name);
        // Create a new room at each spawn.
        foreach(RoomSpawner spawn in spawns)
        {
            if(!spawn.lowerPrio && !spawn.obstructed)
            {
                // Standard Room generation
                if(spawn.requirements.Count <= 0)
                {
                    GameObject room = Instantiate(GetRandomRoom(spawn.doorDirection), spawn.transform.position, Quaternion.identity);
                    room.GetComponent<RoomManager>().directionBuiltFrom = spawn.doorDirection;

                    // Add ourself as a connect room on the new room and vice versa.
                    //room.GetComponent<RoomManager>().connectedRooms.Add(this);
                    //connectedRooms.Add(room.GetComponent<RoomManager>());
                }
                // We are overlayed on another spawn and now have a requirement to take into account generation.
                else
                {
                    GameObject room = Instantiate(GetRandomRoom(spawn.doorDirection, spawn.requirements), spawn.transform.position, Quaternion.identity);
                    room.GetComponent<RoomManager>().directionBuiltFrom = spawn.doorDirection;
                    room.GetComponent<RoomManager>().requirements = spawn.requirements;

                    // Add ourself as a connect room on the new room and vice versa.
                    //room.GetComponent<RoomManager>().connectedRooms.Add(this);
                    //connectedRooms.Add(room.GetComponent<RoomManager>());
                    // Add the other room that was a requirement for the spawn here.
                    //ObstructionLockCheck(spawn.requirements, room.GetComponent<RoomManager>(), true);
                }
            }
            Destroy(spawn.gameObject);
        }
    }

    // Used to see if this placed room is compatable with the other rooms already placed around it.
    private IEnumerator CheckRoomCompatibility()
    {
        //Debug.Log("this is checking the room compatibility");
        yield return new WaitForSeconds(ROOM_COMPATIBILITY_DELAY);
        bool compatibleRoom = false;

        while (!compatibleRoom)
        {
            // Create a list of volume that has spawns in the correct direction.
            List<RoomVolume> volumeWithDesiredSpawnDirection = new List<RoomVolume>();
            // Check if any of the volume shave a connected spawner that matches the direction we are looking for.
            foreach (RoomVolume volume in spaceOccupied)
            {
                //Debug.Log("This volume has " + volume.connectedSpawners.Count + " connected spawns.");
                // Check each volume for it's connected spawns, if a spawn matches the criteria this volume is then added to be tried as the anchor of the room.
                if (directionBuiltFrom != RoomSpawner.DoorOpening.Null)
                {
                    foreach (RoomSpawner connectedSpawn in volume.connectedSpawners)
                    {
                        if (connectedSpawn.doorDirection == FlipValue(directionBuiltFrom))
                            volumeWithDesiredSpawnDirection.Add(volume);
                    }
                }
                else
                    volumeWithDesiredSpawnDirection.Add(volume);
            }
            Vector3 anchor = transform.position;

            //Debug.Log(gameObject + " has " + volumeWithDesiredSpawnDirection.Count + " potential starting points. It should have 1. the origin is " + anchor);
            // For each potential volume starting point, we check to see if there are any issues with spawns or obstructions from our room volume.
            foreach (RoomVolume chosenStartPoint in volumeWithDesiredSpawnDirection)
            {
                compatibleRoom = true;
                // reset the room to the anchor point.
                transform.position = anchor;
                // shift our room to the desired location based on the chosen start point of our volume.
                Vector3 positionalDifference = anchor - chosenStartPoint.transform.position;
                transform.position += positionalDifference;
                // reset all the collision data from our spawns and volume then  wait a frame for them to regain enw data before checking for issues.
                foreach (RoomVolume volume in spaceOccupied)
                    volume.ClearVolumeData();
                foreach (RoomSpawner spawn in spawns)
                    spawn.ClearSpawnData();
                foreach (RoomSpawner spawn in obstructedSpawns)
                    if (spawn != null)
                        spawn.ClearSpawnData();
                obstructedSpawns = new List<RoomSpawner>();
                ObstructionLockCheck(requirements, this, true, chosenStartPoint);
                // We need to wait until there are collision between the spawns and whatnot.
                yield return new WaitForFixedUpdate();

                // Check all the spawns of the room. If any are obstructed, we need to pick a new room.
                foreach (RoomSpawner spawn in spawns)
                {
                    //Debug.Log("We are checking the spawns");
                    if (spawn.obstructed && !spawn.ignoreObstruction)
                    {
                        // Check if this spawn is the spawn that appears in the room that spawne this one.
                        //if (spawn.doorDirection != FlipValue(directionBuiltFrom))
                        compatibleRoom = false;
                    }
                }
                
                // Check all the room volumes, only if we have more than one. If any are obstructed, assuming we arent already replacing this room
                foreach (RoomVolume volume in spaceOccupied)
                {
                    if (volume.obstructed)
                        compatibleRoom = false;
                }
                /*
                //Check to see if this room has more room spawners than is allotted by the game Manager.
                if(((float) GameManager.instance.rooms.Count / (float) GameManager.instance.roomTarget > 0.7f) && spawns.Count > 2)
                {
                    //Debug.Log("this room has too many spawns");
                    compatibleRoom = false;
                }
                */

                // If the room is not compatible, we iterate through the loop again. If it is, we break from it.
                if (compatibleRoom)
                    break;
            }

            //Debug.Log(gameObject + " room compatibility is now: " + compatibleRoom);

            // If the room was not compatible, we will pick a new one that is.
            if (!compatibleRoom)
            {
                GameObject tempRoom = null;
                // Check to see if any of our spawns are overlapped and we need a door way in that direction.

                // Grab a new random room and boof the old one. After grabbign a new room we would check if it fit our criteria.
                if (requirements.Count == 0)
                    tempRoom = GetRandomRoom(directionBuiltFrom);
                else
                    tempRoom = GetRandomRoom(directionBuiltFrom, requirements);

                // Reset to our anchor in case we were slid around to a different room volume.
                transform.position = anchor;

                // Create the temp room we deemed worthy, then add it's intila direction to it.
                GameObject room = Instantiate(tempRoom, transform.position, Quaternion.identity);
                room.GetComponent<RoomManager>().directionBuiltFrom = directionBuiltFrom;
                room.GetComponent<RoomManager>().requirements = requirements;
                //ObstructionLockCheck(requirements, room.GetComponent<RoomManager>(), true);

                //Debug.Log(gameObject + " has a direction of: " + directionBuiltFrom + ", while " + tempRoom + " has a direction of: " + tempRoom.GetComponent<RoomManager>().directionBuiltFrom);

                // Check if any of our spawns had additional priority data we need to tweak now that this room is getting boofed or destroyed.
                foreach (RoomSpawner spawn in spawns)
                    spawn.RemovePriorityData();
                foreach (RoomSpawner spawn in obstructedSpawns)
                    if (spawn != null)
                        spawn.ClearSpawnData();
                obstructedSpawns = new List<RoomSpawner>();


                //GameObject.Find("GameManager").GetComponent<GameManager>().currentRoomGenTimer = 0;

                Destroy(gameObject);
            }
            else
            {
                Invoke("GenerateDungeon", ROOM_GENERATION_DELAY);
                // add this room to the game manager to populate later.
                //GameObject.Find("GameManager").GetComponent<GameManager>().AddRoom(this);
                renderable = transform.Find("Renderable").gameObject;
            }
        }
    }

    // Used to get a random room based a direction
    private GameObject GetRandomRoom(RoomSpawner.DoorOpening direction)
    {
        GameObject room = null;
        switch (direction)
        {
            case RoomSpawner.DoorOpening.TopLeft:
                room = roomBank.allRoomsTL[Random.Range(0, roomBank.allRoomsTL.Length)];
                break;
            case RoomSpawner.DoorOpening.TopRight:
                room = roomBank.allRoomsTR[Random.Range(0, roomBank.allRoomsTR.Length)];
                break;
            case RoomSpawner.DoorOpening.BottomLeft:
                room = roomBank.allRoomsBL[Random.Range(0, roomBank.allRoomsBL.Length)];
                break;
            case RoomSpawner.DoorOpening.BottomRight:
                room = roomBank.allRoomsBR[Random.Range(0, roomBank.allRoomsBR.Length)];
                break;
            default:
                break;
        }
        return room;
    }

    // Used to get a random room based on a direction and some other opening the room will need.
    private GameObject GetRandomRoom(RoomSpawner.DoorOpening direction, List<RoomSpawner.DoorOpening> extraRequirements)
    {
        GameObject room = null;
        bool validRoom = false;

        // AS long as we have not found a valid room, continue through this loop.
        while(!validRoom)
        {
            // Grab a random room
            switch (direction)
            {
                case RoomSpawner.DoorOpening.TopLeft:
                    room = roomBank.allRoomsTL[Random.Range(0, roomBank.allRoomsTL.Length)];
                    break;
                case RoomSpawner.DoorOpening.TopRight:
                    room = roomBank.allRoomsTR[Random.Range(0, roomBank.allRoomsTR.Length)];
                    break;
                case RoomSpawner.DoorOpening.BottomLeft:
                    room = roomBank.allRoomsBL[Random.Range(0, roomBank.allRoomsBL.Length)];
                    break;
                case RoomSpawner.DoorOpening.BottomRight:
                    room = roomBank.allRoomsBR[Random.Range(0, roomBank.allRoomsBR.Length)];
                    break;
                default:
                    break;
            }
            // We a list of volumes with spawns in the proper direction.
            List<RoomVolume> compatibleVolumes = new List<RoomVolume>();
            // Check the spawns of each volume and see if it's in the proper direction, if so add it to our list.
            foreach(RoomVolume currentVolume in room.GetComponent<RoomManager>().spaceOccupied)
            {
                bool openingInProperDirection = false;
                foreach(RoomSpawner spawn in currentVolume.connectedSpawners)
                {
                    if (spawn.doorDirection == FlipValue(directionBuiltFrom))
                        openingInProperDirection = true;
                }
                if (openingInProperDirection)
                    compatibleVolumes.Add(currentVolume);
            }

            RoomManager roomManager = room.GetComponent<RoomManager>();
            foreach (RoomVolume volume in compatibleVolumes)
            {
                bool allRequirementsMet = true;
                // Check to see if any of the spawns m,eet our requirements.
                foreach (RoomSpawner.DoorOpening requirement in extraRequirements)
                {
                    // First we need to flip the requirement around. This is because we are grabbing the spawn on the opposite side of the one that was marked.
                    RoomSpawner.DoorOpening flippedRequirement = FlipValue(requirement);
                    // The requirement is set as false until we can verify the room is properly set up.
                    bool requirementMet = false;
                    foreach (RoomSpawner spawn in volume.connectedSpawners)
                    {
                        if (spawn.doorDirection == flippedRequirement)
                        {
                            //spawn.ignoreObstruction = true;
                            requirementMet = true;
                            break;
                        }
                    }

                    // If the requirement is not met, we 
                    if (!requirementMet)
                    {
                        allRequirementsMet = false;
                        break;
                    }
                }
                // If all the requirements were met this is a valid room.
                if (allRequirementsMet)
                    validRoom = true;
            }
        }
        return room;
    }

    // Used to switch the obstruction ignore locks on on certain spawns of an instance of a room we want to spawn without mpdfying the prefab.
    private void ObstructionLockCheck(List<RoomSpawner.DoorOpening> roomRequirements, RoomManager targetRoom, bool flipRequirement, RoomVolume chosenOrigin)
    {
        //Debug.Log("Obstruction Locks are being actviated.");
        // For every requirement, we will try to find the spawn we will have to obstruction Lock.
        foreach (RoomSpawner.DoorOpening requirement in roomRequirements)
        {
            // First we need to flip the requirement around. This is because we are grabbing the spawn on the opposite side of the one that was marked.
            RoomSpawner.DoorOpening flippedRequirement = requirement;
            if (flipRequirement)
                flippedRequirement = FlipValue(requirement);
            foreach (RoomSpawner spawn in targetRoom.spawns)
            {
                // If a spawn matches it, we set the obstructionLock on it and then break from the loop to check the next requriement.
                if (spawn.doorDirection == flippedRequirement)
                {
                    spawn.ignoreObstruction = true;
                    break;
                }
            }
        }

        // Check all the spawns, if one of them is the opposite of the door direction, that is the room that built us
        // ignore it. If the room has multiple room volumes, there is potential for spawns in the same direction so we will have to check to see if
        // this spawn is on the primary room volumke used as the origin.
        /*
        foreach (RoomSpawner spawn in targetRoom.spawns)
        {
            if (spawn.doorDirection == FlipValue(directionBuiltFrom))
                spawn.ignoreObstruction = true;
        }
        */
        // Check the target room volume to see if it matches the opposite direction of our direction built in.
        foreach(RoomSpawner spawn in chosenOrigin.connectedSpawners)
        {
            if (spawn.doorDirection == FlipValue(directionBuiltFrom))
                spawn.ignoreObstruction = true;
        }
    }

    // Used to switch the DoorOpeningDirection to it's opposite.
    private RoomSpawner.DoorOpening FlipValue(RoomSpawner.DoorOpening originalValue)
    {
        RoomSpawner.DoorOpening newValue = RoomSpawner.DoorOpening.Null;
        switch (originalValue)
        {
            case RoomSpawner.DoorOpening.TopLeft:
                newValue = RoomSpawner.DoorOpening.BottomRight;
                break;
            case RoomSpawner.DoorOpening.TopRight:
                newValue = RoomSpawner.DoorOpening.BottomLeft;
                break;
            case RoomSpawner.DoorOpening.BottomLeft:
                newValue = RoomSpawner.DoorOpening.TopRight;
                break;
            case RoomSpawner.DoorOpening.BottomRight:
                newValue = RoomSpawner.DoorOpening.TopLeft;
                break;
            default:
                break;
        }
        return newValue;
    }

    // This is used to flip every single adjacency checker on then off to see which rooms were adjacent to.
    public void LaunchAdjacencyChecker()
    {
        //Debug.Log("LaunchinAdjacency Checkers");
        foreach (RoomAdjacencyChecker adjacencyChecker in adjacencyCheckers)
            StartCoroutine("FlickerAdjacencyCheckers", adjacencyChecker);
    }

    // This is the coroutine that flicekrs every individual adjacency checker.
    IEnumerator FlickerAdjacencyCheckers(RoomAdjacencyChecker adjacenyChecker)
    {
        adjacenyChecker.gameObject.SetActive(true);
        yield return new WaitForFixedUpdate();
        adjacenyChecker.gameObject.SetActive(false);
    }

    // This method is used when the player enters a room and i 
    public void ShowAdjacentRooms()
    {
        transform.Find("Renderable").gameObject.SetActive(true);
        transform.Find("NonRenderable").gameObject.SetActive(false);

        // Show every connected room in our list
        foreach (RoomManager rm in connectedRooms)
        {
            //Debug.Log("Showing room renderable for: " + rm.gameObject.name);
            rm.transform.Find("Renderable").gameObject.SetActive(true);
            rm.transform.Find("NonRenderable").gameObject.SetActive(false);
        }
    }

    // This method hides this room if it was previously being rendered
    public void HideRoom()
    { 
        //Debug.Log("we are hiding this room");
        transform.Find("Renderable").gameObject.SetActive(false);
        transform.Find("NonRenderable").gameObject.SetActive(true);
        foreach (DoorBehaviour door in connectedDoors)
        {
            if (door.doorState != DoorBehaviour.DoorState.Closed)
                door.InteractWithDoor(true);
        }
    }

    // USed when we open a door, close all the connected doors in this room and all adjacent rooms if they are not already close.
    public void DoorOpen(DoorBehaviour openedDoor)
    {
        Debug.Log("WE ARE NOW CLOSING ALL OTHER DOORS IN THIS ROOM");
        // This closes each of the doors directly connected to this rooom that are not the door we just interacted with.
        foreach (DoorBehaviour door in connectedDoors)
            if (door != openedDoor)
            {
                Debug.Log("This door is not the one we just opened: " + door.gameObject.name);
                if (door.doorState != DoorBehaviour.DoorState.Closed)
                {
                    Debug.Log("this door is not closed and would be closed here");
                    door.InteractWithDoor(true);
                }
                else
                    Debug.Log("this door is closed and were golden.");
            }
        
        Debug.Log("WE ARE NOW CLOSING ALL DOORS IN ADJACENT ROOMS");
        // This closes the rest of the doors for the adjacent rooms if they are active in the hierarchy.
        foreach (RoomManager room in connectedRooms)
            foreach (DoorBehaviour door in room.connectedDoors)
                if (door.gameObject.activeInHierarchy && door.doorState != DoorBehaviour.DoorState.Closed)
                {
                    Debug.Log(door.gameObject.name + " is not closed and in an adjacent room. its parent is: " + door.transform.parent.name);
                    door.InteractWithDoor(true);
                }
                    

    }
}
