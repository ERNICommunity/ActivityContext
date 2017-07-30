using System;
using System.Collections.Generic;
using System.IO;

namespace ActivityContext.Serialization
{
    /// <summary>
    /// Provides methods for serialization of activities into JSON data format.
    /// </summary>
    public static class ActivityJsonSerializer
    {
        public static string Serialize(IEnumerable<Activity> activities)
        {
            using (var output = new StringWriter())
            {
                Write(activities, output);
                return output.ToString();
            }
        }

        /// <summary>
        /// Serializes the given activities and writes the JSON to provided TextWriter.
        /// </summary>
        /// <param name="activities">Activities to be serialized.</param>
        /// <param name="output">The TextWriter used to write output JSON.</param>
        public static void Write(IEnumerable<Activity> activities, TextWriter output)
        {
            if (activities == null) throw new ArgumentNullException(nameof(activities));
            if (output == null) throw new ArgumentNullException(nameof(output));

            output.Write('[');

            bool isFirst = true;
            foreach (var activity in activities)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    output.Write(',');
                }

                Write(activity, output);
            }

            output.Write(']');
        }

        /// <summary>
        /// Serializes the given activity and writes the JSON to provided TextWriter.
        /// </summary>
        /// <param name="activity">Activity to be serialized.</param>
        /// <param name="output">The TextWriter used to write output JSON.</param>
        public static void Write(Activity activity, TextWriter output)
        {
            if (activity == null) throw new ArgumentNullException(nameof(activity));

            output.Write("{\"name\":\"");
            JsonUtils.JsonEscapeFast(activity.Name, output);
            output.Write("\",\"id\":\"");
            output.Write(activity.Id);
            output.Write("\"}");
        }
    }
}