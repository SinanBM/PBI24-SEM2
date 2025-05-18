using System.Globalization;
using Nexttech.Models;


    public class Vector3
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public Vector3(float x, float y, float z) { X = x; Y = y; Z = z; }
    }

    public class Triangle
    {
        public Vector3 Normal { get; }
        public Vector3 Vertex1 { get; }
        public Vector3 Vertex2 { get; }
        public Vector3 Vertex3 { get; }

        public Triangle(Vector3 normal, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            Normal = normal ?? throw new ArgumentNullException(nameof(normal));
            Vertex1 = vertex1 ?? throw new ArgumentNullException(nameof(vertex1));
            Vertex2 = vertex2 ?? throw new ArgumentNullException(nameof(vertex2));
            Vertex3 = vertex3 ?? throw new ArgumentNullException(nameof(vertex3));
        }
    }

public class StlParser
{
    public List<Triangle> Triangles { get; private set; } = new List<Triangle>();

    public void Parse(string filePath)
    {
        if (IsAsciiStl(filePath))
            ParseAscii(filePath);
        else
            ParseBinary(filePath);
    }

    private bool IsAsciiStl(string filePath)
    {
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(fs);

        char[] buffer = new char[80];
        int readChars = reader.Read(buffer, 0, 80);
        string header = new string(buffer, 0, readChars);

        if (!header.StartsWith("solid", StringComparison.OrdinalIgnoreCase))
            return false;

        fs.Position = 0;
        byte[] bytes = new byte[Math.Min(256, fs.Length)];
        fs.Read(bytes, 0, bytes.Length);
        foreach (byte b in bytes)
        {
            if (b > 127) return false;
        }
        return true;
    }

    private void ParseAscii(string filePath)
    {
        Triangles.Clear();

        using var reader = new StreamReader(filePath);
        string? line;

        Vector3? normal = null;
        var vertices = new List<Vector3>();

        while ((line = reader.ReadLine()) != null)
        {
            line = line.Trim();
            if (line.StartsWith("facet normal"))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                normal = new Vector3(
                    float.Parse(parts[2], CultureInfo.InvariantCulture),
                    float.Parse(parts[3], CultureInfo.InvariantCulture),
                    float.Parse(parts[4], CultureInfo.InvariantCulture));
                vertices.Clear();
            }
            else if (line.StartsWith("vertex"))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var vertex = new Vector3(
                    float.Parse(parts[1], CultureInfo.InvariantCulture),
                    float.Parse(parts[2], CultureInfo.InvariantCulture),
                    float.Parse(parts[3], CultureInfo.InvariantCulture));
                vertices.Add(vertex);
            }
            else if (line.StartsWith("endfacet"))
            {
                if (normal != null && vertices.Count == 3)
                {
                    var tri = new Triangle(normal, vertices[0], vertices[1], vertices[2]);
                    Triangles.Add(tri);
                }
                normal = null;
                vertices.Clear();
            }
        }
    }

    private void ParseBinary(string filePath)
    {
        Triangles.Clear();

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var br = new BinaryReader(fs);

        br.ReadBytes(80);
        uint triangleCount = br.ReadUInt32();

        for (uint i = 0; i < triangleCount; i++)
        {
            float normalX = br.ReadSingle();
            float normalY = br.ReadSingle();
            float normalZ = br.ReadSingle();

            float v1X = br.ReadSingle();
            float v1Y = br.ReadSingle();
            float v1Z = br.ReadSingle();

            float v2X = br.ReadSingle();
            float v2Y = br.ReadSingle();
            float v2Z = br.ReadSingle();

            float v3X = br.ReadSingle();
            float v3Y = br.ReadSingle();
            float v3Z = br.ReadSingle();

            br.ReadUInt16(); // attribute byte count

            var tri = new Triangle(
                new Vector3(normalX, normalY, normalZ),
                new Vector3(v1X, v1Y, v1Z),
                new Vector3(v2X, v2Y, v2Z),
                new Vector3(v3X, v3Y, v3Z)
            );

            Triangles.Add(tri);
        }
    }

    private Vector3 Cross(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);
    }

    private Vector3 Subtract(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    // get geometry from stl file
    public (float Length, float Width, float Height) GetBoundingBoxDimensions()
    {
        if (Triangles.Count == 0)
            throw new InvalidOperationException("No triangles parsed.");

        float minX = float.MaxValue, minY = float.MaxValue, minZ = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue, maxZ = float.MinValue;

        foreach (var tri in Triangles)
        {
            var vertices = new[] { tri.Vertex1, tri.Vertex2, tri.Vertex3 };
            foreach (var v in vertices)
            {
                if (v.X < minX) minX = v.X;
                if (v.Y < minY) minY = v.Y;
                if (v.Z < minZ) minZ = v.Z;
                if (v.X > maxX) maxX = v.X;
                if (v.Y > maxY) maxY = v.Y;
                if (v.Z > maxZ) maxZ = v.Z;
            }
        }

        float length = maxX - minX; // X-axis
        float width = maxY - minY;  // Y-axis
        float height = maxZ - minZ; // Z-axis

        return (length, width, height);
    }

    public double CalculateVolume()
    {
        double volume = 0;
        foreach (var tri in Triangles)
        {
            var v1 = tri.Vertex1;
            var v2 = tri.Vertex2;
            var v3 = tri.Vertex3;

            volume += (v1.X * (v2.Y * v3.Z - v3.Y * v2.Z) -
                       v1.Y * (v2.X * v3.Z - v3.X * v2.Z) +
                       v1.Z * (v2.X * v3.Y - v3.X * v2.Y));
        }
        volume /= 6.0;
        return Math.Abs(volume);
    }

    public bool IsWatertight()
    {
        var edgeCounts = new Dictionary<(int, int), int>();
        var vertexList = new List<Vector3>();
        var vertexIndices = new Dictionary<string, int>();

        int GetVertexIndex(Vector3 v)
        {
            string key = $"{v.X:F6}_{v.Y:F6}_{v.Z:F6}";
            if (!vertexIndices.TryGetValue(key, out int index))
            {
                index = vertexList.Count;
                vertexList.Add(v);
                vertexIndices[key] = index;
            }
            return index;
        }

        foreach (var tri in Triangles)
        {
            int i1 = GetVertexIndex(tri.Vertex1);
            int i2 = GetVertexIndex(tri.Vertex2);
            int i3 = GetVertexIndex(tri.Vertex3);

            void AddEdge(int a, int b)
            {
                var edge = a < b ? (a, b) : (b, a);
                if (edgeCounts.ContainsKey(edge))
                    edgeCounts[edge]++;
                else
                    edgeCounts[edge] = 1;
            }

            AddEdge(i1, i2);
            AddEdge(i2, i3);
            AddEdge(i3, i1);
        }

        foreach (var kvp in edgeCounts)
        {
            if (kvp.Value != 2)
                return false;
        }
        return true;
    }
    public StlInputDto ParseToDto(string path)
    {
        Parse(path);
        if (Triangles.Count == 0)
            throw new InvalidOperationException("No triangles found in STL file.");

        var (length, width, height) = GetBoundingBoxDimensions();

        return new StlInputDto
        {
            TriangleCount = Triangles.Count,
            Volume = CalculateVolume(),
            Length = length,
            Width = width,
            Height = height,
            IsWatertight = IsWatertight()
        };
    }



}

