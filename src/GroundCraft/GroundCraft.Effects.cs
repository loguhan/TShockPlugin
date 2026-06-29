using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace GroundCraft;

public sealed partial class GroundCraft
{
    private void SpawnCraftEffect(Vector2 center)
    {
        CraftEffectSpec effect = _config.CraftEffect;
        if (!effect.Enabled || effect.BurstCount <= 0)
            return;

        float radius = effect.RadiusTiles * 16f;
        string style = effect.StyleOrDefault();
        for (int i = 0; i < effect.BurstCount; i++)
        {
            Vector2 point = center + EffectOffset(i, effect.BurstCount, radius);
            if (style is "Fairy" or "Both")
                NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 2, (int)point.X, (int)point.Y, 0f, effect.FairyColor);

            if (style is "Smoke" or "Both")
                NetMessage.SendData(MessageID.PoofOfSmoke, -1, -1, null, (int)point.X, point.Y);
        }
    }

    private static Vector2 EffectOffset(int index, int total, float radius)
    {
        if (total <= 1 || radius <= 0f)
            return Vector2.Zero;

        float angle = MathHelper.TwoPi * index / total;
        return new Vector2(MathF.Cos(angle) * radius, MathF.Sin(angle) * radius);
    }
}
