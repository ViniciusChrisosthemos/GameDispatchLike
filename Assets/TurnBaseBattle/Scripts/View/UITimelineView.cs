using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UITimelineView : MonoBehaviour
{
    [SerializeField] private UIListDisplay _characterIcons;

    private List<UITimelineChracterView> _timelineCharacterControllers;

    private TimelineController _timelineController;

    public void SetTimeline(TimelineController timelineController)
    {
        _timelineController = timelineController;

        _timelineController.OnTimelineUpdated += HandleTimelineUpdated;
        _timelineController.OnDequeue += HandleTimelineDequeue;

        HandleTimelineUpdated();
    }

    private void HandleTimelineDequeue(ITimelineElement element)
    {
        var controller = _timelineCharacterControllers.Find(c => c.GetItem<ITimelineElement>() == element);

        controller.SetSelected();

        var queue = _timelineController.GetTimeline();

        foreach (var c in _timelineCharacterControllers)
        {
            var e = c.GetItem<ITimelineElement>();

            if (e != element && !queue.Contains(e))
            {
                c.SetUnavailable();
            }
        }
    }

    private void HandleTimelineUpdated()
    {
        var elements = _timelineController.GetElements().Select(e => e as object).ToList();

        _characterIcons.SetItems(elements, null);

        _timelineCharacterControllers = new List<UITimelineChracterView>();
        foreach (var controller in _characterIcons.GetControllers())
        {
            _timelineCharacterControllers.Add(controller.GetComponent<UITimelineChracterView>());
        }
    }
}
