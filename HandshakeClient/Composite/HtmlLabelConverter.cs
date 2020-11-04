using HandshakeClient.ViewModels;
using HandshakeClient.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HandshakeClient.Composite
{
  public class HtmlLabelConverter : IValueConverter
  {
    #region Fields

    internal static readonly Regex HashtagGroupRegex = new Regex("#(?<name>[a-zA-Z0-9]*)", RegexOptions.Compiled);
    internal static readonly Regex HyperlinkRegex = new Regex(@"(https|http)?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&\/\/=]*)", RegexOptions.Compiled);

    private ICommand openGroupCommand = new Command<string>(async (groupName) =>
    {
      await Shell.Current.GoToAsync($"{nameof(GroupDetailPage)}?{nameof(GroupDetailViewModel.Name)}={groupName}");
    });

    private ICommand openUrlCommand = new Command<string>(async (url) =>
    {
      await Launcher.OpenAsync(new Uri(url));
    });

    #endregion Fields

    #region Enums

    private enum CommandType
    {
      NoCommand,
      Url,
      Group
    }

    #endregion Enums

    #region Methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var formatted = new FormattedString();

      foreach (var section in ProcessString((string)value))
        formatted.Spans.Add(CreateSpan(section));

      return formatted;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    private IList<StringSection> ProcessString(string rawText)
    {
      var sections = new List<StringSection>();
      if(string.IsNullOrEmpty(rawText))
      {
        return sections;
      }

      var matches1 = HyperlinkRegex.Matches(rawText).OfType<Match>().Select(o => new MatchContainer() { Match = o, CommandType = CommandType.Url });
      var matches2 = HashtagGroupRegex.Matches(rawText).OfType<Match>().Select(o => new MatchContainer() { Match = o, CommandType = CommandType.Group });
      var allMatches = matches1.Concat(matches2).ToList();

      var lastIndex = 0;

      foreach (var item in allMatches)
      {
        var match = item.Match;

        if (match.Index < lastIndex)
        {
          continue;
        }

        var foundText = match.Value;
        sections.Add(new StringSection() { Text = rawText.Substring(lastIndex, match.Index) });
        lastIndex += match.Index + match.Length;

        var value = match.Value;

        StringSection section;
        if (item.CommandType == CommandType.Url)
        {
          section = new StringSection()
          {
            Text = value,
            Parameter = value,
            CommandType = item.CommandType
          };
        }
        else
        {
          section = new StringSection()
          {
            Text = value,
            Parameter = match.Groups["name"].Value,
            CommandType = item.CommandType
          };
        }

        sections.Add(section);
      }

      sections.Add(new StringSection() { Text = rawText.Substring(lastIndex) });

      return sections;
    }

    private Span CreateSpan(StringSection section)
    {
      var span = new Span()
      {
        Text = section.Text
      };

      switch (section.CommandType)
      {
        case CommandType.Url:
          span.GestureRecognizers.Add(new TapGestureRecognizer()
          {
            Command = openUrlCommand,
            CommandParameter = section.Parameter
          });
          span.TextColor = (Color)App.Current.Resources["LinkTextColor"];
          break;
        case CommandType.Group:
          span.GestureRecognizers.Add(new TapGestureRecognizer()
          {
            Command = openGroupCommand,
            CommandParameter = section.Parameter
          });
          span.TextColor = (Color)App.Current.Resources["LinkTextColor"];
          break;
      }

      return span;
    }

    #endregion Methods

    #region Classes

    private class MatchContainer
    {
      #region Properties

      public CommandType CommandType { get; set; }
      public Match Match { get; set; }

      #endregion Properties
    }

    private class StringSection
    {
      #region Properties

      public CommandType CommandType { get; set; }
      public string Parameter { get; set; }
      public string Text { get; set; }

      #endregion Properties
    }

    #endregion Classes
  }
}