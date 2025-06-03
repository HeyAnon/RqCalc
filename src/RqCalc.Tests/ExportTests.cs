using RqCalc.Tests._Base;

namespace RqCalc.Tests;

[TestClass]
public class ExportTests : TestBase
{
    [TestMethod]
    public void ExportEquipment()
    {
        using (var connection = new SQLiteConnection(Properties.Settings.Default.ConnectionString))
        using (var dataSource = new RQDBContext(connection, false, ImplementTypeResolver.Default.WithCache()))
        {
            var request = from equipment in dataSource.Equipments

                where !equipment.IsPersonal

                select new
                {
                    Name = equipment.Name,

                    TypeName = equipment.Type.Name,

                    SlotName = equipment.Type.Slot.Name
                };

            var res = request.ToArray();

            File.WriteAllLines(@"D:\equipment.cvs", res.Select(obj => new[] { obj.Name, obj.TypeName, obj.SlotName }.Join(",", str => $"\"{str}\"")));

            return;
        }
    }

    [TestMethod]
    public void ExportCard()
    {
        using (var connection = new SQLiteConnection(Properties.Settings.Default.ConnectionString))
        using (var dataSource = new RQDBContext(connection, false, ImplementTypeResolver.Default.WithCache()))
        {
            var request = from card in dataSource.Cards.ToList()

                where card.Type.Name != "Blood"

                let isWeapon = card.EquipmentSlots.Select(e => e.Slot).Select(slot => slot.IsWeapon).Distinct().SingleMaybe().GetValueOrDefault()

                let slot = card.EquipmentSlots.Select(e => e.Slot).SingleMaybe().GetValueOrDefault()

                select new
                {
                    Name = card.Name,

                    TypeName = card.Type.Name,

                    SlotName = slot?.Name ?? "",

                    IsWeapon = isWeapon
                };

            var res = request.ToArray();

            File.WriteAllLines(@"D:\card.cvs", res.Select(obj => new[] { obj.Name, obj.TypeName, obj.SlotName, obj.IsWeapon?.ToString() ?? "" }.Join(",", str => $"\"{str}\"")));

            return;
        }
    }

    [TestMethod]
    public void ExportToJson()
    {
        using (var connection = new SQLiteConnection(Properties.Settings.Default.ConnectionString))
        using (var dataSource = new RQDBContext(connection, false, ImplementTypeResolver.Default.WithCache()))
        {
            var request = from card in dataSource.Cards.ToList()

                where card.Type.Name != "Blood"

                let isWeapon = card.EquipmentSlots.Select(e => e.Slot).Select(slot => slot.IsWeapon).Distinct().SingleMaybe().GetValueOrDefault()

                let slot = card.EquipmentSlots.Select(e => e.Slot).SingleMaybe().GetValueOrDefault()

                select new
                {
                    Name = card.Name,

                    TypeName = card.Type.Name,

                    SlotName = slot?.Name ?? "",

                    IsWeapon = isWeapon
                };

            var res = request.ToArray();

            File.WriteAllLines(@"D:\card.cvs", res.Select(obj => new[] { obj.Name, obj.TypeName, obj.SlotName, obj.IsWeapon?.ToString() ?? "" }.Join(",", str => $"\"{str}\"")));

            return;
        }
    }
}