using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day16
    {
        public class Packet
        {
            public int Version { get; set; }
            public int ID { get; set; }
        }

        public class LiteralPacket : Packet
        {
            public long Number { get; set; }
        }

        public class OperatorPacket : Packet
        {
            public int LengthType { get; set; }
            public int Length { get; set; }
            public List<Packet> SubPackets { get; set; } = new List<Packet>();
        }

        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day16.txt");

            string file = reader.ReadToEnd();

            byte[] data = new byte[file.Length / 2];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = byte.Parse(file.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            List<Packet> packets = new List<Packet>();
            ReadPackets(data, 0, data.Length * 8, packets);

            int sum = SumVersions(packets);

            Console.WriteLine(sum);
        }

        private int SumVersions(List<Packet> packets)
        {
            return packets.Sum(p => p.Version)
                + packets.Where(p => p is OperatorPacket).Sum(p => SumVersions((p as OperatorPacket).SubPackets));
        }

        private int ReadPackets(byte[] data, int idx, int length, List<Packet> packets)
        {
            int end = idx + length;
            while (end - idx > 8)
            {
                int version = ReadBits(data, idx, 3);
                int id = ReadBits(data, idx + 3, 3);

                idx += 6;

                switch (id)
                {
                    case 4:
                        var literal = new LiteralPacket()
                        {
                            Version = version,
                            ID = id
                        };

                        idx = ReadNumber(literal, data, idx);
                        packets.Add(literal);
                        break;

                    default:
                        var operatorPacket = new OperatorPacket()
                        {
                            Version = version,
                            ID = id
                        };

                        int lengthType = ReadBits(data, idx, 1);
                        int packetLengthLength = lengthType == 0 ? 15 : 11;
                        int packetsLength = ReadBits(data, idx + 1, packetLengthLength);

                        operatorPacket.Length = packetsLength;
                        operatorPacket.LengthType = lengthType;

                        idx += 1 + packetLengthLength;

                        switch (lengthType)
                        {
                            case 0:
                                idx = ReadPackets(data, idx, packetsLength, operatorPacket.SubPackets);
                                break;
                            default:
                                idx = ReadNPackets(data, idx, packetsLength, operatorPacket.SubPackets);
                                break;
                        }

                        packets.Add(operatorPacket);
                        break;
                }
            }

            return idx;
        }

        private int ReadNPackets(byte[] data, int idx, int length, List<Packet> packets)
        {
            int end = idx + length;

            for(int i = 0; i < length; i++)
            {
                int version = ReadBits(data, idx, 3);
                int id = ReadBits(data, idx + 3, 3);

                idx += 6;

                switch (id)
                {
                    case 4:
                        var literal = new LiteralPacket()
                        {
                            Version = version,
                            ID = id
                        };

                        idx = ReadNumber(literal, data, idx);
                        packets.Add(literal);
                        break;

                    default:
                        var operatorPacket = new OperatorPacket()
                        {
                            Version = version,
                            ID = id
                        };

                        int lengthType = ReadBits(data, idx, 1);
                        int packetLengthLength = lengthType == 0 ? 15 : 11;
                        int packetsLength = ReadBits(data, idx + 1, packetLengthLength);

                        operatorPacket.Length = packetsLength;
                        operatorPacket.LengthType = lengthType;

                        idx += 1 + packetLengthLength;

                        switch (lengthType)
                        {
                            case 0:
                                idx = ReadPackets(data, idx, packetsLength, operatorPacket.SubPackets);
                                break;
                            default:
                                idx = ReadNPackets(data, idx, packetsLength, operatorPacket.SubPackets);
                                break;
                        }

                        packets.Add(operatorPacket);
                        break;
                }
            }

            return idx;
        }

        private int ReadNumber(LiteralPacket packet, byte[] data, int idx)
        {
            int read = 0;
            long number = 0;

            int indicator;

            do
            {
                indicator = ReadBits(data, idx, 1);
                number = (number << 4) | ReadBits(data, idx + 1, 4);
                idx += 5;
            } while (indicator != 0);

            packet.Number = number;
            return idx;
        }

        private int ReadBits(byte[] data, int pos, int length)
        {
            int idx = (pos / 8);

            int read = 0;
            int res = 0;
            int offset;

            if (pos % 8 != 0)
            {
                offset = 8 - (pos % 8);
                res = data[idx++] & ((1 << offset) - 1);
                read = offset;
            }

            if (read > length)
            {
                res >>= (read - length);
                return res;
            }

            while (read + 8 < length)
            {
                res = (res << 8) | data[idx++];
                read += 8;
            }

            if (read != length)
            {
                offset = length - read;
                res = (res << offset) | ((data[idx] >> (8 - offset)) & ((1 << offset) - 1));
            }

            return res;
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day16.txt");

            string file = reader.ReadToEnd();

            byte[] data = new byte[file.Length / 2];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = byte.Parse(file.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            List<Packet> packets = new List<Packet>();
            ReadPackets(data, 0, data.Length * 8, packets);

            long result = EvaluatePackets(packets[0]);

            Console.WriteLine(result);
        }

        private long EvaluatePackets(Packet packet)
        {
            long result = 0;

            if (packet is LiteralPacket l)
            {
                return l.Number;
            }
            else if (packet is OperatorPacket o)
            {
                var results = o.SubPackets.Select(o => EvaluatePackets(o)).ToList();

                switch (packet.ID)
                {
                    case 0:
                        return results.Sum();

                    case 1:
                        result = 1;
                        foreach (long num in results)
                        {
                            result *= num;
                        }
                        return result;

                    case 2:
                        result = int.MaxValue;
                        foreach (long num in results)
                        {
                            result = Math.Min(num, result);
                        }
                        return result;

                    case 3:
                        result = int.MinValue;
                        foreach (long num in results)
                        {
                            result = Math.Max(num, result);
                        }
                        return result;

                    case 5:
                        return results[0] > results[1] ? 1 : 0;

                    case 6:
                        return results[0] < results[1] ? 1 : 0;

                    case 7:
                        return results[0] == results[1] ? 1 : 0;

                    default:
                        throw new Exception("Invalid packet type");
                }
            }

            throw new Exception("Invalid packet type");
        }
    }
}
