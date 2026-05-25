using System;
using Zenject;
using HMUI;
using UnityEngine;
using IPA.Utilities;
using BeatSaberMarkupLanguage.Components;

namespace ImageCoverExpander
{
    public class ArtworkViewManager : IInitializable, IDisposable
    {
        private StandardLevelDetailViewController _standardLevelViewController;
        private MainMenuViewController _mainMenuViewController;

        private static readonly Vector3 modifiedSizeDelta = new Vector2(70.5f, 58);
        private static readonly Vector3 modifiedPositon = new Vector3(-34.4f, -56f, 0f);
        private static readonly float modifiedSkew = 0;

        public ArtworkViewManager(StandardLevelDetailViewController standardLevelDetailViewController, MainMenuViewController mainMenuViewController)
        {
            _standardLevelViewController = standardLevelDetailViewController;
            _mainMenuViewController = mainMenuViewController;
        }

        public void Initialize()
        {
            _mainMenuViewController.didFinishEvent += OnDidFinishEvent;
        }

        public void Dispose()
        {
            _mainMenuViewController.didFinishEvent -= OnDidFinishEvent;
        }

        private void OnDidFinishEvent(MainMenuViewController _, MainMenuViewController.MenuButton __)
        {
            var levelBarTranform = _standardLevelViewController.transform.Find("LevelDetail").Find("LevelBarBig");
            if (!levelBarTranform) { return; }
            Plugin.Log.Notice("Changing artwork for " + levelBarTranform.name);
            try
            {
                var imageTransform = levelBarTranform.Find("SongArtwork").GetComponent<RectTransform>();
                imageTransform.sizeDelta = modifiedSizeDelta;
                imageTransform.localPosition = modifiedPositon;
                imageTransform.SetAsFirstSibling();

                var imageView = imageTransform.GetComponent<ImageView>();
                imageView.color = new Color(0.5f, 0.5f, 0.5f, 1);
                imageView.preserveAspect = false;
                FieldAccessor<ImageView, float>.Set(ref imageView, "_skew", modifiedSkew);

                // For DiTails
                var clickableImage = imageTransform.GetComponent<ClickableImage>();
                if (clickableImage != null)
                {
                    clickableImage.DefaultColor = new Color(0.4f, 0.4f, 0.4f, 1);
                }
            }
            catch (Exception e)
            {
                Plugin.Log.Error("Error changing artwork fields for " + levelBarTranform.name);
                Plugin.Log.Error(e);
            }
            // CreateTextOverlay(levelBarTranform);
        }

        //  private void CreateTextOverlay(Transform parent)
        // {
        //     // Create root overlay object parented to the level bar
        //     GameObject overlay = new GameObject("TextOverlay");
        //     overlay.transform.SetParent(parent, false);  // false = maintain world space position
            
        //     // Add and configure background image
        //     var image = overlay.AddComponent<ImageView>();
        //     image.color = new Color(0f, 0f, 0f, 0.8f); // Black with 80% opacity (RGBA)
            
        //     // Critical fix: Match the skew setting from main artwork modifications
        //     // Beat Saber's ImageView has a private "_skew" field that causes parallelogram distortion
        //     FieldAccessor<ImageView, float>.Set(ref image, "_skew", modifiedSkew); // modifiedSkew = 0
            
        //     // Configure positioning and size using RectTransform
        //     RectTransform rect = overlay.GetComponent<RectTransform>();
        //     // Anchor values relative to parent container:
        //     // (0.05, -0.8) = 5% from left, 80% below parent's center
        //     // (0.85, -0.2) = 85% from left, 20% below parent's center
        //     rect.anchorMin = new Vector2(0.05f, -0.8f);
        //     rect.anchorMax = new Vector2(0.85f, -0.2f);
            
        //     // Zero out position/size offsets since we're using anchor-based layout
        //     rect.anchoredPosition = Vector2.zero;  // No positional offset from anchors
        //     rect.sizeDelta = new Vector2(0, 0);    // No size modification beyond anchors

        //     // Optional: Adjust render order if needed
        //     // overlay.transform.SetSiblingIndex(1); // Example: Place behind specific elements
            

        // }
    }
}
