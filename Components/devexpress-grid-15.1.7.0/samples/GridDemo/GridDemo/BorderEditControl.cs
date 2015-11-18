using System;
using Xamarin.Forms;
using DevExpress.Mobile.Core;

namespace DevExpress.GridDemo {
	public class BorderDisplayControl : ContentView {

		public static readonly BindableProperty TextProperty = BindingUtils.CreateProperty<BorderDisplayControl, string>(o => o.Text, default(string), OnTextChanged);
		public string Text {
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static void OnTextChanged(BindableObject obj, string oldValue, string newValue) {
			BorderDisplayControl control = obj as BorderDisplayControl;
			control.label.Text = newValue;
		}

		Label label;
		public BorderDisplayControl() {
			ContentView border = new ContentView();
			border.BackgroundColor = Color.Green;
			Content = border;
			label = new Label();
			border.Content = label;
		}
	}

	public class BorderEditControl : ContentView {

		public static readonly BindableProperty TextProperty = BindingUtils.CreateProperty<BorderEditControl, string>(o => o.Text, default(string), OnTextChanged);
		public string Text {
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static void OnTextChanged(BindableObject obj, string oldValue, string newValue) {
			BorderEditControl control = obj as BorderEditControl;
			control.editor.Text = newValue;
		}

		Editor editor;
		public BorderEditControl() {
			ContentView border = new ContentView();
			border.BackgroundColor = Color.Blue;
			Content = border;
			editor = new Editor();
			editor.TextChanged += EditorTextChanged;
			border.Content = editor;
		}

		void EditorTextChanged(object sender, TextChangedEventArgs e) {
			this.Text = editor.Text;
		}
	}
}

