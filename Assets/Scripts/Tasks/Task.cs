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
    public string TaskName { get { return taskName; } private set { taskName = value; } }

    [SerializeField] TaskType type;
    public TaskType Type { get { return type; } private set { type = value; } }

    [SerializeField] Sprite sprite;
    public Sprite SpriteAsset { get {  return sprite; } private set { sprite = value; } }

    [SerializeField][TextArea] string uIText;
    public string UIText { get { return uIText; } private set { uIText = value; } }

    [SerializeField] float completionTime;
    public float CompletionTime { get {  return completionTime; } private set {  completionTime = value; } }

    /*    [SerializeField] Room roomPos;
        public Room RoomPos { get { return roomPos; } set { roomPos = value; } }*/
}
