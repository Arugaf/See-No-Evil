using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
namespace Tutorial
{
    public class CompositeTutorialSection : ITutorialSection
    {
        private ITutorialSection[] sections;
        public CompositeTutorialSection(IEnumerable<ITutorialSection> sections)
        {
            this.sections = sections.ToArray();
        }
        public CompositeTutorialSection(params ITutorialSection[] sections)
        {
            this.sections = sections;
        }

        public async UniTask Perform(ITutorialView view)
        {
            foreach (ITutorialSection sec in sections)
            {
                view.Progress = 0;
                await sec.Perform(view);
            }
        }
    }
}