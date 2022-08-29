﻿using TimberbornAPI.Common;
using TimberbornAPI.UIBuilderSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    /// <summary>
    /// Handles the creation of new floodgate-streamgauge link views.
    /// </summary>
    public class StreamGaugeFloodgateLinkViewFactory
    {
        private readonly UIBuilder _builder;
        public StreamGaugeFloodgateLinkViewFactory(
            UIBuilder builder)
        {
            _builder = builder;
        }

        /// <summary>
        /// Create a link view that is shown on the Floodgate's fragment
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VisualElement CreateViewForFloodgate(int index)
        {
            var foo = _builder.CreateComponentBuilder()
                              .CreateVisualElement()
                              .AddComponent(
                                _builder.CreateComponentBuilder()
                                        .CreateButton()
                                        .SetName("Target")
                                        .AddClass("entity-fragment__button")
                                        .AddClass("entity-fragment__button--green")
                                        .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                        .SetFontSize(new Length(14, LengthUnit.Pixel))
                                        .SetFontStyle(FontStyle.Normal)
                                        .SetHeight(new Length(30, LengthUnit.Pixel))
                                        .SetWidth(new Length(290, LengthUnit.Pixel))
                                        .SetPadding(new Padding(0, 0, 0, 0))
                                        .SetMargin(new Margin(new Length(2, LengthUnit.Pixel), 0, new Length(2, LengthUnit.Pixel), 0))
                                        .AddComponent(
                                            _builder.CreateComponentBuilder()
                                                    .CreateVisualElement()
                                                    .SetFlexWrap(Wrap.Wrap)
                                                    .SetFlexDirection(FlexDirection.Row)
                                                    .SetJustifyContent(Justify.SpaceBetween)
                                                    .AddComponent(
                                                         _builder.CreateComponentBuilder()
                                                                 .CreateVisualElement()
                                                                 .SetFlexWrap(Wrap.Wrap)
                                                                 .SetFlexDirection(FlexDirection.Row)
                                                                 .SetJustifyContent(Justify.FlexStart)
                                                                 .AddComponent(
                                                                      _builder.CreateComponentBuilder()
                                                                              .CreateVisualElement()
                                                                              .SetName("ImageContainer")
                                                                              .SetWidth(new Length(28, LengthUnit.Pixel))
                                                                              .SetHeight(new Length(28, LengthUnit.Pixel))
                                                                              .SetMargin(new Margin(new Length(1, LengthUnit.Pixel), 0, 0, new Length(6, LengthUnit.Pixel)))
                                                                              .Build())
                                                                 .AddPreset(factory => factory.Labels()
                                                                                              .GameTextBig(text: "Stream Gauge",
                                                                                                           builder: builder => builder.SetWidth(new Length(100, LengthUnit.Pixel))))
                                                                 .Build())
                                                    .AddComponent(
                                                        _builder.CreateComponentBuilder()
                                                              .CreateButton()
                                                              .AddClass("unity-text-element")
                                                              .AddClass("unity-button")
                                                              .AddClass("entity-panel__button")
                                                              .AddClass("entity-panel__button--red")
                                                              .AddClass("distribution-route__icon-wrapper")
                                                              .SetName("DetachLinkButton")
                                                              .SetMargin(new Margin(new Length(1, LengthUnit.Pixel), new Length(2, LengthUnit.Pixel), 0, 0))
                                                              .AddComponent(_builder.CreateComponentBuilder()
                                                                                    .CreateVisualElement()
                                                                                    .AddClass("entity-panel__button")
                                                                                    .AddClass("delete-building__icon")
                                                                                    .Build())
                                                              .Build())
                                                    .Build())
                                              .Build())
                                  .AddComponent(
                                        _builder.CreateComponentBuilder()
                                                .CreateVisualElement()
                                                .AddPreset(
                                                    factory => factory.Labels()
                                                                      .GameTextBig(name: $"Threshold1Label{index}",
                                                                                   locKey: "Floodgates.Triggers.Threshold1",
                                                                                   builder: 
                                                                                    builder => builder.SetStyle(
                                                                                        style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: $"Threshold1Slider{index}",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, LengthUnit.Pixel), 0))))
                                                .AddPreset(
                                                    factory => factory.Labels()
                                                                      .GameTextBig(name: $"Threshold1FloodgateHeightLabel{index}",
                                                                                   locKey: "Floodgates.Triggers.HeightWhenBelowThreshold1",
                                                                                   builder:
                                                                                    builder => builder.SetStyle(
                                                                                        style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: $"Threshold1FloodgateHeightSlider{index}",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, LengthUnit.Pixel), 0))))
                                                .AddPreset(
                                                    factory => factory.Labels()
                                                                      .GameTextBig(name: $"Threshold2Label{index}",
                                                                                   locKey: "Floodgates.Triggers.Threshold2",
                                                                                   builder: 
                                                                                    builder => builder.SetStyle(
                                                                                        style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: $"Threshold2Slider{index}",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, LengthUnit.Pixel), 0))))
                                                .AddPreset(
                                                    factory => factory.Labels()
                                                                      .GameTextBig(name: $"Threshold2FloodgateHeightLabel{index}",
                                                                                   locKey: "Floodgates.Triggers.HeightWhenAboveThreshold2",
                                                                                   builder:
                                                                                    builder => builder.SetStyle(
                                                                                        style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: $"Threshold2FloodgateHeightSlider{index}",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, LengthUnit.Pixel), 0))))
                                                .Build())
                                  .Build();

            return foo;
        }

        /// <summary>
        /// Createa a link view that is shown on the StreamGauge's fragment
        /// </summary>
        /// <returns></returns>
        public VisualElement CreateViewForStreamGauge()
        {
            var foo = _builder.CreateComponentBuilder()
                                     .CreateButton()
                                     .SetName("Target")
                                     .AddClass("entity-fragment__button")
                                     .AddClass("entity-fragment__button--green")
                                     .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                     .SetFontSize(new Length(14, LengthUnit.Pixel))
                                     .SetFontStyle(FontStyle.Normal)
                                     .SetHeight(new Length(30, LengthUnit.Pixel))
                                     .SetWidth(new Length(290, LengthUnit.Pixel))
                                     .SetPadding(new Padding(0, 0, 0, 0))
                                     .SetMargin(new Margin(new Length(2, LengthUnit.Pixel), 0, new Length(2, LengthUnit.Pixel), 0))
                                     .AddComponent(
                                         _builder.CreateComponentBuilder()
                                                 .CreateVisualElement()
                                                 .SetFlexWrap(Wrap.Wrap)
                                                 .SetFlexDirection(FlexDirection.Row)
                                                 .SetJustifyContent(Justify.SpaceBetween)
                                                 .AddComponent(
                                                      _builder.CreateComponentBuilder()
                                                              .CreateVisualElement()
                                                              .SetFlexWrap(Wrap.Wrap)
                                                              .SetFlexDirection(FlexDirection.Row)
                                                              .SetJustifyContent(Justify.FlexStart)
                                                              .AddComponent(
                                                                   _builder.CreateComponentBuilder()
                                                                           .CreateVisualElement()
                                                                           .SetName("ImageContainer")
                                                                           .SetWidth(new Length(28, LengthUnit.Pixel))
                                                                           .SetHeight(new Length(28, LengthUnit.Pixel))
                                                                           .SetMargin(new Margin(new Length(1, LengthUnit.Pixel), 0, 0, new Length(6, LengthUnit.Pixel)))
                                                                           .Build())
                                                              .AddPreset(factory => factory.Labels()
                                                                                           .GameTextBig(text: "Floodgate",
                                                                                                        builder: builder => builder.SetWidth(new Length(100, LengthUnit.Pixel))))
                                                              .Build())
                                                 .AddComponent(
                                                     _builder.CreateComponentBuilder()
                                                           .CreateButton()
                                                           .AddClass("unity-text-element")
                                                           .AddClass("unity-button")
                                                           .AddClass("entity-panel__button")
                                                           .AddClass("entity-panel__button--red")
                                                           .AddClass("distribution-route__icon-wrapper")
                                                           .SetName("DetachLinkButton")
                                                           .SetMargin(new Margin(new Length(1, LengthUnit.Pixel), new Length(2, LengthUnit.Pixel), 0, 0))
                                                           .AddComponent(_builder.CreateComponentBuilder()
                                                                                 .CreateVisualElement()
                                                                                 .AddClass("entity-panel__button")
                                                                                 .AddClass("delete-building__icon")
                                                                                 .Build())
                                                           .Build())
                                                 .Build())
                                     .Build();

            return foo;
        }
    }
}
