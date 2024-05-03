using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "ScriptableObjects/Tasks", order = 1)]
public class Task : ScriptableObject
{
    public enum TaskType
    {
        Cleaning = 0,
        PickingUp = 1,
        NumOfTasks
    }

/*    public enum Room
    {
        Entrance,
        SideRoom1,
        SideRoom2,
        NumOfRooms
    }*/

    [SerializeField] string taskName;
    public string TaskName { get { return taskName; } set { taskName = value; } }

    [SerializeField] TaskType type;
    public TaskType Type { get { return type; } set { type = value; } }

/*    [SerializeField] Room roomPos;
    public Room RoomPos { get { return roomPos; } set { roomPos = value; } }*/
}
