using ArmyClash.Battle.Services;
using ArmyClash.Battle.UI.Data;
using ArmyClash.MegaWorldGrid;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;
using Zenject;

namespace ArmyClash.Battle.UI.Actions
{
    [RequiresData(typeof(BattleUIViewData))]
    [Name("UI/ArmyClash/RandomizeButtonAction")]
    public sealed class RandomizeButtonAction : UIToolkitAction
    {
        [Inject]
        private GridSpawnerPair _spawner;

        [Inject]
        private BattleStateService _state;

        protected override void OnFirstSetupComponentUI(object[] setupData = null)
        {
            BattleUIViewData view = Get<BattleUIViewData>();
            view.RandomizeButton.clicked += OnClicked;
            LogClickedSubscribers(view.RandomizeButton, "OnFirstSetupComponentUI");
        }

        protected override void SetupComponent(object[] setupData = null)
        {
            BattleUIViewData view = Get<BattleUIViewData>();
            LogClickedSubscribers(view.RandomizeButton, "SetupComponent");
        }

        protected override void OnDisableElement()
        {
            return;
            BattleUIViewData view = Get<BattleUIViewData>();
            view.RandomizeButton.clicked -= OnClicked;
            LogClickedSubscribers(view.RandomizeButton, "OnDisableElement");
        }

        private void OnClicked()
        {
            _state?.SetIdle();
            _spawner?.RespawnBoth();
        }

        private static void LogClickedSubscribers(Button button, string stage)
        {
            if (button == null)
            {
                Debug.LogWarning($"[RandomizeButtonAction] {stage}: button is null.");
                return;
            }

            Clickable clickable = button.clickable;
            if (clickable == null)
            {
                Debug.LogWarning($"[RandomizeButtonAction] {stage}: clickable is null for button '{button.name}'.");
                return;
            }

            MulticastDelegate clickedDelegate = GetClickedDelegate(clickable);
            if (clickedDelegate == null)
            {
                Debug.Log($"[RandomizeButtonAction] {stage}: no clicked subscribers for '{button.name}'.");
                return;
            }

            Delegate[] handlers = clickedDelegate.GetInvocationList();
            var sb = new StringBuilder();
            for (int i = 0; i < handlers.Length; i++)
            {
                Delegate handler = handlers[i];
                object target = handler.Target;
                sb.Append(i + 1)
                    .Append(". ")
                    .Append(handler.Method.DeclaringType?.FullName)
                    .Append('.')
                    .Append(handler.Method.Name)
                    .Append(" | target=")
                    .Append(target?.GetType().FullName ?? "static")
                    .AppendLine();
            }

            Debug.Log(
                $"[RandomizeButtonAction] {stage}: clicked subscribers for '{button.name}' ({handlers.Length}):\n{sb}");
        }

        private static MulticastDelegate GetClickedDelegate(Clickable clickable)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            string[] possibleFieldNames = { "clicked", "m_Clicked", "<clicked>k__BackingField" };

            for (int i = 0; i < possibleFieldNames.Length; i++)
            {
                FieldInfo field = typeof(Clickable).GetField(possibleFieldNames[i], flags);
                if (field?.GetValue(clickable) is MulticastDelegate clickedDelegate)
                {
                    return clickedDelegate;
                }
            }

            return null;
        }
    }
}
