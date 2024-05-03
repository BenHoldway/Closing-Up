using UnityEngine;

public class TaskPicker : MonoBehaviour
{
    Task[] tasks;
    int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Initialises the task array
    public void InitArray(int numOfTasks)
    {
        tasks = new Task[numOfTasks];
    }

    //Constructs the task
    public void PickTask(Task task)
    {
        if (tasks == null)
            return;

        tasks[index] = task;
        index++;
    }
}
