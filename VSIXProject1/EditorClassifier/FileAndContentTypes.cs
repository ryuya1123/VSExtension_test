using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text;
using System.Linq.Expressions;
using System.Windows.Media;
using System.Windows;

namespace VSIXProject1.EditorClassifier
{
	internal class FileAndContentTypes
	{
	}

	internal static class FileAndContentTypeDefinitions
	{
		[Export]
		[Name("hid")]
		[BaseDefinition("text")]
		internal static ContentTypeDefinition hidingContentTypeDefinition;

		[Export]
		[FileExtension(".hid")]
		[ContentType("hid")]
		internal static FileExtensionToContentTypeDefinition hiddenFileExtensionDefinition;

		[Export(typeof(IGlyphFactoryProvider))]
		[Name("TodoGlyph")]
		[Order(After = "VsTextMarker")]
		[ContentType("code")]
		[TagType(typeof(TodoTag))]
		internal sealed class TodoGlyphFactoryProvider : IGlyphFactoryProvider
		{
            public IGlyphFactory GetGlyphFactory(IWpfTextView view, IWpfTextViewMargin margin)
            {
                return new TodoGlyphFactory();
            }
        }

		[Export(typeof(ITaggerProvider))]
		[ContentType("code")]
		[TagType(typeof(TodoTag))]
		class TodoTaggerProvider : ITaggerProvider
		{
			[Import]
			internal IClassifierAggregatorService AggregatorService;

			public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
			{
				if(buffer == null)
				{
					throw new ArgumentNullException("buffer");
				}
				return new TodoTagger(AggregatorService.GetClassifier(buffer)) as ITagger<T>;
			}
		}

		[Export(typeof(IWpfTextViewCreationListener))]
		[ContentType("text")]
		[TextViewRole(PredefinedTextViewRoles.Document)]
		internal class testViewCreationListener : IWpfTextViewCreationListener
		{
			[Import]
			internal IEditorFormatMapService FormatMapService = null;

			public void TextViewCreated(IWpfTextView textView)
			{
				IEditorFormatMap formatMap = FormatMapService.GetEditorFormatMap(textView);

                ResourceDictionary regularCaretProperties = formatMap.GetProperties("Caret");
                ResourceDictionary overwriteCaretProperties = formatMap.GetProperties("Overwrite Caret");
                ResourceDictionary indicatorMargin = formatMap.GetProperties("Indicator Margin");
                ResourceDictionary visibleWhitespace = formatMap.GetProperties("Visible Whitespace");
                ResourceDictionary selectedText = formatMap.GetProperties("Selected Text");
                ResourceDictionary inactiveSelectedText = formatMap.GetProperties("Inactive Selected Text");

                formatMap.BeginBatchUpdate();

                regularCaretProperties[EditorFormatDefinition.ForegroundBrushId] = Brushes.Magenta;
                formatMap.SetProperties("Caret", regularCaretProperties);

                overwriteCaretProperties[EditorFormatDefinition.ForegroundBrushId] = Brushes.Turquoise;
                formatMap.SetProperties("Overwrite Caret", overwriteCaretProperties);

                indicatorMargin[EditorFormatDefinition.BackgroundColorId] = Colors.LightGreen;
                formatMap.SetProperties("Indicator Margin", indicatorMargin);

                visibleWhitespace[EditorFormatDefinition.ForegroundColorId] = Colors.Red;
                formatMap.SetProperties("Visible Whitespace", visibleWhitespace);

                selectedText[EditorFormatDefinition.BackgroundBrushId] = Brushes.LightPink;
                formatMap.SetProperties("Selected Text", selectedText);

                inactiveSelectedText[EditorFormatDefinition.BackgroundBrushId] = Brushes.DeepPink;
                formatMap.SetProperties("Inactive Selected Text", inactiveSelectedText);

                formatMap.EndBatchUpdate();
            }
		}
	}
}
