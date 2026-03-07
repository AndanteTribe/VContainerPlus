#nullable enable

using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace VContainerPlus.Editor
{
    /// <summary>
    /// Custom inspector for <see cref="LifetimeScopeBase"/> that provides validation and prefab reference checking.
    /// </summary>
    [CustomEditor(typeof(LifetimeScopeBase))]
    public class LifetimeScopeBaseInspector : UnityEditor.Editor
    {
        /// <summary>
        /// Creates the inspector GUI with validation button and prefab reference warnings.
        /// </summary>
        /// <returns>The root visual element of the inspector.</returns>
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // Monitor changes to autoInjectGameObjects and _autoBindComponents, and display a warning if a prefab asset is referenced.
            root.RegisterCallback<SerializedPropertyChangeEvent>(static evt =>
            {
                var changedProperty = evt.changedProperty;
                if (changedProperty.propertyPath.StartsWith("autoInjectGameObjects", StringComparison.Ordinal)
                    || changedProperty.propertyPath.StartsWith("_autoBindComponents", StringComparison.Ordinal))
                {
                    var root = ((VisualElement)evt.currentTarget).parent;
                    AddErrorIfPrefab(root, changedProperty);
                }
            });

            var button = new Button { text = "Validation" };
            button.RegisterCallback<ClickEvent, LifetimeScopeBase>(static (evt, instance) =>
            {
                var result = DependencyValidator.Validate(instance);
                var root = ((VisualElement)evt.currentTarget).parent;
                AddErrorIfNotValidate(root, result);
            }, (LifetimeScopeBase)target);
            root.Add(button);
            return root;
        }

        /// <summary>
        /// Checks if a serialized property references a prefab asset and displays an error if so.
        /// </summary>
        /// <param name="root">The root visual element to add the error message to.</param>
        /// <param name="valueProperty">The serialized property to check.</param>
        protected static void AddErrorIfPrefab(VisualElement root, SerializedProperty valueProperty)
        {
            const string warningClassName = "error-object-ref";
            foreach (var warning in root.Query<HelpBox>(className: warningClassName).Build())
            {
                warning.RemoveFromHierarchy();
            }

            var obj = valueProperty.objectReferenceValue;
            if (obj == null)
            {
                return;
            }

            var go = obj switch
            {
                GameObject g => g,
                Component c => c.gameObject,
                _ => null
            };

            if (go == null)
            {
                return;
            }

            // Check if it's a prefab asset.
            if (PrefabUtility.IsPartOfPrefabAsset(go))
            {
                var helpBox = new HelpBox("You are referencing a Prefab asset. Please reference a scene object.", HelpBoxMessageType.Error);
                helpBox.AddToClassList(warningClassName);
                root.Add(helpBox);
            }
        }

        /// <summary>
        /// Displays validation errors in the inspector if the validation result is invalid.
        /// </summary>
        /// <param name="root">The root visual element to add the error message to.</param>
        /// <param name="result">The validation result to check.</param>
        protected static void AddErrorIfNotValidate(VisualElement root, ValidateResult result)
        {
            const string warningClassName = "error-validate-result";
            foreach (var warning in root.Query<HelpBox>(className: warningClassName).Build())
            {
                warning.RemoveFromHierarchy();
            }

            if (result.IsValid)
            {
                return;
            }

            var helpBox = new HelpBox(result.FormatFailedResults(), HelpBoxMessageType.Error);
            helpBox.AddToClassList(warningClassName);
            root.Add(helpBox);
        }
    }
}