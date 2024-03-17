using UnityEngine;
using UnityEngine.UIElements;

namespace ShalicoUtils.Editor
{
    public static class StyleUtility
    {
        public static void UnityFieldContainerStyle(IStyle target)
        {
            target.flexDirection = FlexDirection.Row;
            target.flexShrink = 0;
            target.overflow = Overflow.Hidden;
            target.marginTop = 0;
            target.marginBottom = 0;
            target.marginLeft = 3;
            target.marginRight = -2;

            target.whiteSpace = WhiteSpace.NoWrap;
        }

        public static void UnityFieldLabelStyle(IStyle target)
        {
            target.minWidth = 150;
            target.paddingLeft = 1;
            target.paddingTop = 2;
            target.paddingRight = 0;
            target.paddingBottom = 0;

            target.marginTop = 0;
            target.marginBottom = 0;
            target.marginRight = 0;

            target.flexGrow = 0;
            target.flexShrink = 0;
            target.flexBasis = StyleKeyword.Auto;

            target.unityOverflowClipBox = OverflowClipBox.ContentBox;
            target.whiteSpace = WhiteSpace.NoWrap;
        }

        public static void UnityFieldValueStyle(IStyle target)
        {
            target.flexGrow = 1;
            target.flexShrink = 1;
            target.flexBasis = StyleKeyword.Auto;

            target.unityOverflowClipBox = OverflowClipBox.ContentBox;

            target.unityTextAlign = TextAnchor.MiddleLeft;

            target.marginLeft = 0;
            target.marginTop = 0;
            target.marginRight = 0;
            target.marginBottom = 0;

            target.paddingLeft = 1;
            target.paddingRight = 0;
            target.paddingTop = 0;
            target.paddingBottom = 0;
        }
    }
}