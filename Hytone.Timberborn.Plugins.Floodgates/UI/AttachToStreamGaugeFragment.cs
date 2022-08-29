﻿using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Timberborn.Common;
using Timberborn.Localization;
using Timberborn.SelectionSystem;
using Timberborn.WaterBuildings;
using TimberbornAPI.Common;
using TimberbornAPI.UIBuilderSystem;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class AttachToStreamGaugeFragment
    {
        private readonly UIBuilder _builder;
        private readonly ILoc _loc;
        private readonly AttachToStreamGaugeButton _attachToStreamGaugeButton;
        private FloodgateTriggerMonoBehaviour _floodgateTriggerMonoBehaviour;

        private VisualElement _linksScrollView;
        private Label _noLinks;
        private Sprite _streamGaugeSprite;

        private readonly SelectionManager _selectionManager;
        StreamGaugeFloodgateLinkViewFactory _streamGaugeFloodgateLinkViewFactory;

        //There has to be a better way for this...
        private List<Tuple<Label, Slider, Label, Slider, Label, Slider, Label, Tuple<Slider>>> _settingsList = new List<Tuple<Label, Slider, Label, Slider, Label, Slider, Label, Tuple<Slider>>>();

        public AttachToStreamGaugeFragment(
            AttachToStreamGaugeButton attachToStreamGaugeButton,
            UIBuilder builder,
            SelectionManager selectionManager,
            StreamGaugeFloodgateLinkViewFactory streamGaugeFloodgateLinkViewFactory,
            ILoc loc)
        {
            _attachToStreamGaugeButton = attachToStreamGaugeButton;
            _builder = builder;
            _selectionManager = selectionManager;
            _streamGaugeFloodgateLinkViewFactory = streamGaugeFloodgateLinkViewFactory;
            _loc = loc;
        }


        public VisualElement InitiliazeFragment(VisualElement parent)
        {
            _streamGaugeSprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                  .Where(x => x.name.StartsWith("StreamGauge"))
                                                  .SingleOrDefault();

            var root = _builder.CreateComponentBuilder()
                               .CreateVisualElement()
            .SetName("LinksScrollView")
                               .SetWidth(new Length(290, LengthUnit.Pixel))
                               .SetJustifyContent(Justify.Center)
                               .SetMargin(new Margin(0, 0, new Length(7, LengthUnit.Pixel), 0))
                               .BuildAndInitialize();

            _attachToStreamGaugeButton.Initialize(parent, () => _floodgateTriggerMonoBehaviour, delegate
            {
                RemoveAllStreamGaugeViews();
                //AddAllStreamGaugeViews();
                ShowFragment(_floodgateTriggerMonoBehaviour);
            });

            _noLinks = _builder.CreateComponentBuilder()
                               .CreateLabel()
                               .AddPreset(factory => factory.Labels()
                                                            .GameTextBig(name: "NoLinksLabel",
                                                                         locKey: "Floodgates.Triggers.NoLinks",
                                                                         builder: builder =>
                                                                            builder.SetStyle(style =>
                                                                                style.alignSelf = Align.Center)))
                               .BuildAndInitialize();

            _linksScrollView = root.Q<VisualElement>("LinksScrollView");

            return root;
        }

        public void ClearFragment()
        {
            _floodgateTriggerMonoBehaviour = null;
            _settingsList.Clear();
            RemoveAllStreamGaugeViews();
        }

        public void ShowFragment(FloodgateTriggerMonoBehaviour floodgateTriggerMonoBehaviour)
        {
            _floodgateTriggerMonoBehaviour = floodgateTriggerMonoBehaviour;
            AddAllStreamGaugeViews();
            var links = _floodgateTriggerMonoBehaviour.FloodgateLinks;
            for (int i = 0; i < links.Count(); i++)
            {
                var link = links[i];
                var floodgate = link.Floodgate.GetComponent<Floodgate>();
                var setting = _settingsList[i];
                setting.Item2.SetValueWithoutNotify(link.Threshold1);
                setting.Item4.SetValueWithoutNotify(link.Threshold2);
                var height = UIHelpers.GetMaxHeight(floodgate);
                setting.Item6.highValue = height;
                setting.Item6.SetValueWithoutNotify(link.Height1);
                setting.Rest.Item1.SetValueWithoutNotify(link.Height2);
                setting.Rest.Item1.highValue = height;
            }
        }

        public void UpdateFragment()
        {
            if ((bool)_floodgateTriggerMonoBehaviour)
            {
                var links = _floodgateTriggerMonoBehaviour.FloodgateLinks;
                for (int i = 0; i < links.Count(); i++)
                {
                    var setting = _settingsList[i];
                    setting.Item1.text = $"{_loc.T("Floodgates.Triggers.Threshold1")}: {setting.Item2.value.ToString(CultureInfo.InvariantCulture)}";
                    setting.Item3.text = $"{_loc.T("Floodgates.Triggers.Threshold2")}: {setting.Item4.value.ToString(CultureInfo.InvariantCulture)}";
                    setting.Item5.text = $"{_loc.T("Floodgates.Triggers.HeightWhenBelowThreshold1")}: {setting.Item6.value.ToString(CultureInfo.InvariantCulture)}";
                    setting.Item7.text = $"{_loc.T("Floodgates.Triggers.HeightWhenAboveThreshold2")}: {setting.Rest.Item1.value.ToString(CultureInfo.InvariantCulture)}";
                }
            }
        }

        /// <summary>
        /// Creates a little view for every existing floodgate-streamague link
        /// </summary>
        public void AddAllStreamGaugeViews()
        {
            ReadOnlyCollection<StreamGaugeFloodgateLink> links = _floodgateTriggerMonoBehaviour.FloodgateLinks;
            for (int i = 0; i < links.Count; i++)
            {
                var j = i;
                var link = links[i];
                var streamGauge = link.StreamGauge.gameObject;
                var view = _streamGaugeFloodgateLinkViewFactory.CreateViewForFloodgate(i);

                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                img.sprite = _streamGaugeSprite;
                imageContainer.Add(img);

                var targetButton = view.Q<Button>("Target");
                targetButton.clicked += delegate
                {
                    _selectionManager.FocusOn(streamGauge);
                };

                view.Q<Button>("DetachLinkButton").clicked += delegate
                {
                    link.Floodgate.DetachLink(link);
                    ResetLinks();
                };

                var threshold1Label = view.Q<Label>($"Threshold1Label{i}");
                var threshold1Slider = view.Q<Slider>($"Threshold1Slider{i}");
                threshold1Slider.RegisterValueChangedCallback((@event) => ChangeThresholdSlider(@event, j, 0));
                var threshold2Label = view.Q<Label>($"Threshold2Label{i}");
                var threshold2Slider = view.Q<Slider>($"Threshold2Slider{i}");
                threshold2Slider.RegisterValueChangedCallback((@event) => ChangeThresholdSlider(@event, j, 1));

                var threshold1FloodgateLabel = view.Q<Label>($"Threshold1FloodgateHeightLabel{i}");
                var threshold1FloodgateSlider = view.Q<Slider>($"Threshold1FloodgateHeightSlider{i}");
                threshold1FloodgateSlider.RegisterValueChangedCallback((@event) => ChangeHeightSlider(@event, j, 0));
                var threshold2FloodgateLabel = view.Q<Label>($"Threshold2FloodgateHeightLabel{i}");
                var threshold2FloodgateSlider = view.Q<Slider>($"Threshold2FloodgateHeightSlider{i}");
                threshold2FloodgateSlider.RegisterValueChangedCallback((@event) => ChangeHeightSlider(@event, j, 1));

                var foo = Tuple.Create(
                    threshold1Label, threshold1Slider, threshold2Label, threshold2Slider, threshold1FloodgateLabel, threshold1FloodgateSlider, threshold2FloodgateLabel, threshold2FloodgateSlider);

                _settingsList.Add(foo);
                _linksScrollView.Add(view);
            }

            _attachToStreamGaugeButton.UpdateRemainingSlots(links.Count, _floodgateTriggerMonoBehaviour.MaxStreamGaugeLinks);
            if (links.IsEmpty())
            {
                _linksScrollView.Add(_noLinks);
            }
        }

        /// <summary>
        /// Generic logic for when a threshold slider is changed
        /// </summary>
        /// <param name="changeEvent"></param>
        /// <param name="index"></param>
        /// <param name="sliderIndex"></param>
        public void ChangeThresholdSlider(ChangeEvent<float> changeEvent,
                                          int index,
                                          int sliderIndex)
        {
            Slider slider;
            if (sliderIndex == 0)
            {
                slider = _settingsList[index].Item2;
                _floodgateTriggerMonoBehaviour.FloodgateLinks[index].Threshold1 = changeEvent.newValue;
            }
            else
            {
                slider = _settingsList[index].Item4;
                _floodgateTriggerMonoBehaviour.FloodgateLinks[index].Threshold2 = changeEvent.newValue;
            }
            slider.SetValueWithoutNotify(changeEvent.newValue);
        }

        /// <summary>
        /// Generic logic for when a height slider is changed
        /// </summary>
        /// <param name="changeEvent"></param>
        /// <param name="index"></param>
        /// <param name="heightIndex"></param>
        public void ChangeHeightSlider(ChangeEvent<float> changeEvent, int index, int heightIndex)
        {
            Slider slider;
            if (heightIndex == 0)
            {
                slider = _settingsList[index].Item6;
                var num = UpdateHeightSliderValue(slider, changeEvent.newValue);
                _floodgateTriggerMonoBehaviour.FloodgateLinks[index].Height1 = num;
            }
            else
            {
                slider = _settingsList[index].Rest.Item1;
                var num = UpdateHeightSliderValue(slider, changeEvent.newValue);
                _floodgateTriggerMonoBehaviour.FloodgateLinks[index].Height2 = num;
            }
        }

        /// <summary>
        /// Removes the existing link views and builds
        /// them again
        /// </summary>
        public void ResetLinks()
        {
            RemoveAllStreamGaugeViews();
            AddAllStreamGaugeViews();
            UpdateFragment();
        }

        /// <summary>
        /// Removes all the existing link views
        /// </summary>
        public void RemoveAllStreamGaugeViews()
        {
            _linksScrollView.Clear();
        }

        /// <summary>
        /// Makes the height slider work in increments of 0.5
        /// </summary>
        /// <param name="slider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private float UpdateHeightSliderValue(Slider slider, float value)
        {
            float num = Mathf.Round(value * 2f) / 2f;
            slider.SetValueWithoutNotify(num);
            return num;
        }

    }
}
