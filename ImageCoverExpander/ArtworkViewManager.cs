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
                    clickableImage.DefaultColor = new Color(0.5f, 0.5f, 0.5f, 1);
                }
            }
            catch (Exception e)
            {
                Plugin.Log.Error("Error changing artwork fields for " + levelBarTranform.name);
                Plugin.Log.Error(e);
            }
            CreateTextOverlay(levelBarTranform);
        }

        private void CreateTextOverlay(Transform parent)
        {
            // Create a new GameObject for the overlay
            GameObject overlay = new GameObject("TextOverlay");
            overlay.transform.SetParent(parent, false);

            // Add an ImageView (or Unity UI Image) for the semi-transparent background
            var image = overlay.AddComponent<ImageView>();
            image.color = new Color(0f, 0f, 0f, 0.7f); // Black with 70% opacity

            // Configure the RectTransform to your liking
            RectTransform rect = overlay.GetComponent<RectTransform>();
            // Trial-and-error these anchor values/positions to align the overlay
            rect.anchorMin = new Vector2(0.1f, 0.1f);
            rect.anchorMax = new Vector2(0.9f, 0.3f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(0, 0);

            // For layering: overlay.transform.SetSiblingIndex([index]);
            // Increase or decrease this index to place it behind or in front of other UI elements
        }
    }
}
