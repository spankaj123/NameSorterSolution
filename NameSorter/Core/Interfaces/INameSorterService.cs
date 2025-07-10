using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NameSorter.Core.Models;

// Defines the contract for sorting names
namespace NameSorter.Core.Interfaces
{
    public interface INameSorterService
    {
        // Sorts a collection of names by last name, then given names
        IEnumerable<Name> SortNames(IEnumerable<Name> names);
    }
}
