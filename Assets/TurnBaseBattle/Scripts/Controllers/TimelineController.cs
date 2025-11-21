using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TimelineController
{
    private List<ITimelineElement> _elements;
    private Queue<ITimelineElement> _queue;

    public Action OnTimelineUpdated;
    public Action<ITimelineElement> OnDequeue;
    public Action<ITimelineElement> OnItemDeactivated;

    public TimelineController(List<ITimelineElement> elements)
    {
        _elements = elements;
        _queue = new Queue<ITimelineElement>();
    }

    public void UpdateTimeLine()
    {
        _queue.Clear();

        _elements = _elements.OrderByDescending(e => e.GetPriority()).ToList();
        _elements.ForEach(e => _queue.Enqueue(e));

        OnTimelineUpdated?.Invoke();
    }

    public ITimelineElement Dequeue()
    {
        var element = _queue.Dequeue();

        OnDequeue?.Invoke(element);

        return element;
    }

    public void TriggerItemDeactivated(ITimelineElement element)
    {
        OnItemDeactivated?.Invoke(element);
    }

    public int CurrentSize => _queue.Count;
    public int TrueSize => _elements.Count;

    public bool IsEmpty() => _queue.Count == 0;

    public Queue<ITimelineElement> GetTimeline() => _queue;

    public List<ITimelineElement> GetElements() => _elements;
}

