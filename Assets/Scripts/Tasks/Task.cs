using UnityEngine;

public class Task
{
    public enum TaskType
    {
        Cleaning,
        PickingUp,
        Electronic
    }

    public enum Room
    {
        Entrance,
        SideRoom1,
        SideRoom2
    }

    [SerializeField] string name { get {  return name; } set { name = value; } }
    public string Name {  get; set; }

    [SerializeField] string type { get { return type; } set { type = value; } }
    public TaskType Type { get; set; }

    [SerializeField] string roomPos { get { return roomPos; } set { roomPos = value; } }
    public Room RoomPos { get; set; }
}
