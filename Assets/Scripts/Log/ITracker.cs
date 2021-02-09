using System;
using System.Text;

namespace Log
{
    public interface ITracker
    {
        StringBuilder InitiateTracker(StringBuilder csvLogger);
        void SaveToFile(String path, StringBuilder csvLogger);
    }
}