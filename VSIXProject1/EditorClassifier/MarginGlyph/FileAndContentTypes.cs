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
	}
}
