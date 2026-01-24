using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TimelineController<T> where T : ITimelineElement
{
    private List<T> _elements;
    private Queue<ITimelineElement> _queue;

    public Action OnTimelineUpdated;
    public Action<T> OnDequeue;
    public Action<T> OnItemDeactivated;

    public TimelineController(List<T> elements)
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

    public T Dequeue()
    {
        var timelineElement = _queue.Dequeue();
        var element = (T)timelineElement;

        OnDequeue?.Invoke(element);

        return element;
    }

    public void TriggerItemDeactivated(T element)
    {
        OnItemDeactivated?.Invoke(element);
    }

    public int CurrentSize => _queue.Count;
    public int TrueSize => _elements.Count;

    public bool IsEmpty() => _queue.Count == 0;

    public List<T> GetTimeline() => _queue.Select(item => (T)item).ToList();

    public List<T> GetElements() => _elements;
}

