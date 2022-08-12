namespace LibMMD.Model
{
    public class Part
    {
        public Material.MMDMaterial Material { get; set; }
        public int BaseShift { get; set; } 
        public int TriangleIndexNum { get; set; }
    }
}