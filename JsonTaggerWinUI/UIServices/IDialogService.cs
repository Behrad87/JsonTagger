using System.Collections.Generic;
using System.Threading.Tasks;

namespace JsonTaggerWinUI.UIServices;

public interface IDialogService
{
    Task<IReadOnlyList<string>> OpenFolder();
    Task<IReadOnlyList<string>> OpenJsonFiles();
}