using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TimelineController
{
    private List<ITimelineElement> _elements;
    private Queue<ITimelineElement> _queue;

    public TimelineController(List<ITimelineElement> elements)
    {
        _elements = elements;
        _queue = new Queue<ITimelineElement>();
    }

    public void UpdateTimeLine()
    {
        _queue.Clear();
        _elements.Where(e => e.IsActive()).OrderByDescending(e => e.GetPriority()).ToList().ForEach(e => _queue.Enqueue(e));
    }

    public ITimelineElement Dequeue() => _queue.Dequeue();

    public bool IsEmpty() => _queue.Count == 0;

    public Queue<ITimelineElement> GetTimeline() => _queue;
}

