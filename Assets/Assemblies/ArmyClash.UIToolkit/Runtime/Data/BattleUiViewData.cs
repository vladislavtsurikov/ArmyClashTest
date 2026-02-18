using OdinSerializer;
using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Data
{
    [Name("UI/ArmyClash/BattleUiViewData")]
    public sealed class BattleUIViewData : UIToolkitViewData
    {
        [OdinSerialize] private string _startButtonName = "startButton";
        [OdinSerialize] private string _randomizeButtonName = "randomizeButton";
        [OdinSerialize] private string _fastForwardButtonName = "fastForwardButton";
        [OdinSerialize] private string _leftCountName = "leftCount";
        [OdinSerialize] private string _rightCountName = "rightCount";

        public Button StartButton { get; private set; }
        public Button RandomizeButton { get; private set; }
        public Button FastForwardButton { get; private set; }
        public Label LeftCountLabel { get; private set; }
        public Label RightCountLabel { get; private set; }

        protected override void BindElements()
        {
            StartButton = Query<Button>(_startButtonName);
            RandomizeButton = Query<Button>(_randomizeButtonName);
            FastForwardButton = Query<Button>(_fastForwardButtonName);
            LeftCountLabel = Query<Label>(_leftCountName);
            RightCountLabel = Query<Label>(_rightCountName);
        }
    }
}
