using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingStarted
{
    class SfAssistViewHeaderBehavior : Behavior<ContentPage>
    {
        #region Fields
        internal GettingStartedViewModel? viewModel;
        private Border? border;
        int headerHeight = DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.MacCatalyst ? 255 : 416;
        const int editorHeight = 56;
        const int minPadding = 24;
        private ContentPage? contentPage;
        double padding = DeviceInfo.Platform == DevicePlatform.WinUI ? 25 : 125;
        #endregion

        #region Overrides

        protected override void OnAttachedTo(ContentPage bindable)
        {
            contentPage = bindable;
            border = bindable.FindByName<Border>("border");
            viewModel = bindable.BindingContext as GettingStartedViewModel;
#if WINDOWS || MACCATALYST
            contentPage.PropertyChanged += this.SampleView_PropertyChanged!;
# endif
            border.PropertyChanged += this.View_PropertyChanged!;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
#if WINDOWS || MACCATALYST
            contentPage!.PropertyChanged -= this.SampleView_PropertyChanged!;
#endif
            border!.PropertyChanged -= View_PropertyChanged!;
            viewModel = null;
            base.OnDetachingFrom(bindable);
        }
        #endregion

        #region CallBacks
        private void View_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Height" && viewModel != null)
            {
                double top = ((sender as Border)!.Height - editorHeight) / 2 - (headerHeight / 2);
                top = top < minPadding ? minPadding : top;
                viewModel.HeaderPadding = new Thickness(0, top, 0, 0);
            }
        }

#if WINDOWS || MACCATALYST
        private void SampleView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Height" && viewModel != null && contentPage != null)
            {
                double borderHeight = 0;
#if WINDOWS
                borderHeight = contentPage.Height - padding;
#elif MACCATALYST
               borderHeight = contentPage.Height - (2 * padding);
#endif
                border!.HeightRequest = borderHeight;
            }
        }
#endif
        #endregion

    }
}
