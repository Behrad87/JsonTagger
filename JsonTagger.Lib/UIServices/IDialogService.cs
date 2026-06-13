using System.Collections.Generic;
using System.Threading.Tasks;

namespace JsonTagger.UIServices
{
    public interface IDialogService
    {
        Task<IReadOnlyList<string>> OpenJsonFiles();
        Task<IReadOnlyList<string>> OpenFolder();
    }
}
