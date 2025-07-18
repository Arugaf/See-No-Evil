using System.Collections.Generic;
namespace Tutorial
{
    public static class TutorialSectionExtensions
    {
        public static ITutorialSection Concat(this ITutorialSection section, params ITutorialSection[] sections)
        {
            List<ITutorialSection> sectionsNew = new List<ITutorialSection>(){section};
            sectionsNew.AddRange(sections);
            return new CompositeTutorialSection(sectionsNew);
        }
    }
}