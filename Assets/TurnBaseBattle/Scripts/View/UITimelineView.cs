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
    private ITimelineElement _currentElement;

    public void SetTimeline(TimelineController timelineController)
    {
        _timelineController = timelineController;

        _timelineController.OnDequeue += HandleTimelineDequeue;
        _timelineController.OnTimelineUpdated += HandleTimelineUpdated;
        _timelineController.OnItemDeactivated += HandleItemElementDeactivated;

        HandleTimelineUpdated();
    }

    private void HandleTimelineDequeue(ITimelineElement element)
    {
        UITimelineChracterView controller;

        if (_currentElement != null)
        {
            controller = _timelineCharacterControllers.Find(c => c.GetItem<ITimelineElement>() == _currentElement);

            controller.SetUnavailable();
        }

        if (!element.IsActive()) return;

        controller = _timelineCharacterControllers.Find(c => c.GetItem<ITimelineElement>() == element);

        controller.SetSelected();

        _currentElement = element;
    }

    private void HandleTimelineUpdated()
    {
        var elements = _timelineController.GetElements().Where(e => e.IsActive()).ToList();

        _characterIcons.SetItems(elements, null);

        var controllers = _characterIcons.GetControllers();
        _timelineCharacterControllers = new List<UITimelineChracterView>();

        for (int i = 0; i < elements.Count; i++)
        {
            var controller = controllers[i];
            var tmElement = controller.GetItem<ITimelineElement>();
            var tmController = controller.GetComponent<UITimelineChracterView>();

            _timelineCharacterControllers.Add(tmController);

            if (!tmElement.IsActive())
            {
                tmController.SetInative();
            }
        }

        _currentElement = null;
    }

    private void HandleItemElementDeactivated(ITimelineElement element)
    {
        Debug.Log($"HandleItemElementDeactivated   {element}");

        var controller = _timelineCharacterControllers.Find(c => c.GetItem<ITimelineElement>() == element);
        controller.SetInative();
    }
}
