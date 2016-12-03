using System;
using System.IO;

namespace Markdown.Parsing.Tokenizer.Input
{
    public class StreamInput : IInput
    {
        public StreamReader BaseStream { get; }
        public int MaxDistance { get; }

        public bool AtEnd => BaseStream.EndOfStream && FutureString.Length == 0;

        private string PastString { get; }
        private string FutureString { get; }

        public StreamInput(StreamReader baseStream, int maxDistance)
        {
            BaseStream = baseStream;
            MaxDistance = maxDistance;
            FutureString = baseStream.ReadString(maxDistance + 1);
            PastString = FutureString == "" ? "" : FutureString.Substring(0, 1);
        }

        private StreamInput(StreamReader baseStream, int maxDistance, string pastString, string futureString)
        {
            BaseStream = baseStream;
            MaxDistance = maxDistance;
            FutureString = futureString;
            PastString = pastString;
        }

        public string LookAtString(int length)
        {
            ThrowExceptionIfInvalidDistance(length - 1);
            return FutureString.Substring(0, Math.Min(FutureString.Length, length));
        }

        private void ThrowExceptionIfInvalidDistance(int distance)
        {
            if (Math.Abs(distance) > MaxDistance)
                throw new ArgumentException($"Try get access to character on distance {distance} when MaxDistance equal to {MaxDistance}");
            if (distance < 0)
                throw new ArgumentException("Invalid distance/length value");
        }

        public char? LookAhead(int distance)
        {
            ThrowExceptionIfInvalidDistance(distance);
            if (distance < FutureString.Length)
                return FutureString[distance];
            return null;
        }

        public char? LookBehind(int distance)
        {
            ThrowExceptionIfInvalidDistance(distance);
            if (distance < PastString.Length)
                return PastString[PastString.Length - distance - 1];
            return null;
        }

        public IInput Advance(int distance)
        {
            var additionalString = BaseStream.ReadString(distance);
            var fullString = PastString + FutureString.Substring(1) + additionalString;
            var nextFutureStartPosition = Math.Min(distance + PastString.Length - 1, fullString.Length);
            var nextFutureString = "";
            if (nextFutureStartPosition < fullString.Length)
            {
                var nextFutureLength = Math.Min(fullString.Length - nextFutureStartPosition, MaxDistance + 1);
                nextFutureString = fullString.Substring(nextFutureStartPosition, nextFutureLength);
            }

            var nextPastStartPosition = Math.Max(fullString.Length - nextFutureString.Length - MaxDistance, 0);
            var nextPastEndPosition = fullString.Length - nextFutureString.Length;
            var nextPastString = "";
            if (nextFutureString != "")
            {
                nextPastString = fullString.Substring(nextPastStartPosition,
                    nextPastEndPosition - nextPastStartPosition + 1);
            }
            return new StreamInput(BaseStream, MaxDistance, nextPastString, nextFutureString);
        }

        public void Dispose()
        {
            BaseStream.Dispose();
        }
    }
}
