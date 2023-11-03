using System;
using UnityEngine.UI;

////TODO: have updateBindingUIEvent receive a control path string, too (in addition to the device layout name)

namespace UnityEngine.InputSystem.Samples.RebindUI
{
    /// <summary>
    /// This is an example for how to override the default display behavior of bindings. The component
    /// hooks into <see cref="RebindActionUI.updateBindingUIEvent"/> which is triggered when UI display
    /// of a binding should be refreshed. It then checks whether we have an icon for the current binding
    /// and if so, replaces the default text display with an icon.
    /// </summary>
    public class GamepadIconsExample : MonoBehaviour
    {
        public GamepadIcons keyboard;

        protected void OnEnable()
        {
            // Hook into all updateBindingUIEvents on all RebindActionUI components in our hierarchy.
            var rebindUIComponents = transform.GetComponentsInChildren<RebindActionUI>();
            foreach (var component in rebindUIComponents)
            {
                component.updateBindingUIEvent.AddListener(OnUpdateBindingDisplay);
                component.UpdateBindingDisplay();
            }
        }

        protected void OnUpdateBindingDisplay(RebindActionUI component, string bindingDisplayString, string deviceLayoutName, string controlPath)
        {
            if (string.IsNullOrEmpty(deviceLayoutName) || string.IsNullOrEmpty(controlPath))
                return;

            var icon = default(Sprite);

            if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Keyboard"))
            {
                icon = keyboard.GetSprite(controlPath);
            }
                
                

            var textComponent = component.bindingText;

            // Grab Image component.
            var imageGO = textComponent.transform.parent.Find("ActionBindingIcon");
            var imageComponent = imageGO.GetComponent<Image>();

            if (icon != null)
            {
                textComponent.gameObject.SetActive(false);
                imageComponent.sprite = icon;
                imageComponent.gameObject.SetActive(true);
            }
            else
            {
                textComponent.gameObject.SetActive(true);
                imageComponent.gameObject.SetActive(false);
            }
        }

        [Serializable]
        public struct GamepadIcons
        {
            public Sprite A;
            public Sprite B;
            public Sprite C;
            public Sprite D;
            public Sprite E;
            public Sprite F;
            public Sprite G;
            public Sprite H;
            public Sprite I;
            public Sprite J;
            public Sprite K;
            public Sprite L;
            public Sprite M;
            public Sprite N;
            public Sprite O;
            public Sprite P;
            public Sprite Q;
            public Sprite R;
            public Sprite S;
            public Sprite T;
            public Sprite U;
            public Sprite V;
            public Sprite W;
            public Sprite X;
            public Sprite Y;
            public Sprite Z;
            public Sprite Space;
            public Sprite Tab;
            public Sprite Enter;
            //public Sprite Escape;
            public Sprite LeftShift;
            public Sprite RightShift;

            public Sprite GetSprite(string controlPath)
            {
                // From the input system, we get the path of the control on device. So we can just
                // map from that to the sprites we have for gamepads.
                switch (controlPath)
                {
                    case "<keyboard>/a": return A;
                    case "<keyboard>/b": return B;
                    case "<keyboard>/c": return C;
                    case "<keyboard>/d": return D;
                    case "<keyboard>/e": return E;
                    case "<keyboard>/f": return F;
                    case "<keyboard>/g": return G;
                    case "<keyboard>/h": return H;
                    case "<keyboard>/i": return I;
                    case "<keyboard>/j": return J;
                    case "<keyboard>/k": return K;
                    case "<keyboard>/l": return L;
                    case "<keyboard>/m": return M;
                    case "<keyboard>/n": return N;
                    case "<keyboard>/o": return O;
                    case "<keyboard>/p": return P;
                    case "<keyboard>/q": return Q;
                    case "<keyboard>/r": return R;
                    case "<keyboard>/s": return S;
                    case "<keyboard>/t": return T;
                    case "<keyboard>/u": return U;
                    case "<keyboard>/v": return V;
                    case "<keyboard>/w": return W;
                    case "<keyboard>/x": return X;
                    case "<keyboard>/y": return Y;
                    case "<keyboard>/z": return Z;
                    case "<keyboard>/space": return Space;
                    case "<keyboard>/tab": return Tab;
                    case "<keyboard>/enter": return Enter;
                    //case "<Keyboard>/escape": return Escape;
                    case "<Keyboard>/leftShift": return LeftShift;
                    case "<Keyboard>/rightShift": return RightShift;
                }
                return null;
            }
        }
    }
}
