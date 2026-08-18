# Survival Top-Down Prototype (Unity)

Dự án game Prototype Survival Top-Down được xây dựng bằng Unity, đáp ứng 100% các yêu cầu kỹ thuật và gameplay theo đề bài.

---

## 1. Thông tin chung
- **Unity Version**: Unity 2022.3 LTS (hoặc phiên bản Unity bạn đang dùng)
- **Nền tảng mục tiêu**: Mobile (Android / iOS) & Unity Editor Playable
- **Hướng màn hình**: Landscape (Ngang)

---

## 2. Cách mở & Chạy Project
1. Mở Unity Hub và bấm **Add** -> chọn thư mục dự án `Survival-Top-down`.
2. Mở Scene chính tại đường dẫn: `Assets/Scenes/SampleScene.unity`.
3. Nhấn nút **Play** trên Unity Editor để trải nghiệm game.

---

## 3. Phím & Nút điều khiển (Controls)

| Thao tác | Điều khiển trên Editor (PC) | Điều khiển trên Mobile (UI) |
| :--- | :--- | :--- |
| **Di chuyển** | Phím **WASD** hoặc **Mũi tên** | **Virtual Joystick** (`UIJoystick.cs`) trên UI |
| **Đánh thường (Bắn 3 viên)** | Phím **Space** / **J** / Click chuột nút Bắn | Nút **Attack Button** (`ButtonAttack.cs`) trên UI |
| **Kỹ năng 1 (Đặt bom)** | Phím **K** / Click chuột nút Bom | Nút **Bomb Skill Button** (`ButtonBombSkill.cs`, CD 12s) |
| **Kỹ năng 2 (Dash nổ)** | Phím **L** / **Left Shift** / Click nút Dash | Nút **Dash Skill Button** (`ButtonDashExplosionSkill.cs`, CD 6s) |

> **Tính năng Tự động ngắm (Auto-Aim)**: Khi bấm bắn (`PlayerShooting.cs`), nhân vật tự động quét `Physics.OverlapSphere`, ưu tiên chọn và xoay người về phía kẻ địch gần nhất trong góc nón 90° (và fallback 360° nếu xoay quay lưng với quái).

---

## 4. Danh sách tính năng (Status Checklist)

###  PHẦN BẮT BỘC (MANDATORY) — ĐÃ HOÀN THÀNH 100%

- [x] **Nhân vật (Player)**:
  - Chỉ số ban đầu trong `PlayerSO.asset`: Máu 500/500, Tốc độ 2 unit/s, Xoay 180°/s, Giáp 0, Damage Multiplier 0.
  - Công thức sát thương nhận trong `DamageReceiver.cs`: `Sát thương nhận = Sát thương gốc - Giáp` (tối thiểu 0).
  - Công thức sát thương gây ra trong `DamageSender.cs`: `Sát thương gây ra = Sát thương gốc * (1 + Damage Multiplier)`.
  - Xoay nhân vật 180°/s bằng `Quaternion.RotateTowards` trong `PlayerMoving.cs`.

- [x] **Kỹ năng (Skills)**:
  - **Đánh thường (Charge Shot)** (`PlayerShooting.cs` & `TripleShot.asset`): Bắn 3 viên góc nón (-15°, 0°, +15°), 10 ST gốc. Tối đa 3 charge, hồi +1 charge/3s, chống spam 0.5s.
  - **Kỹ năng 1 (Đặt bom)** (`ButtonBombSkill.cs`, `BombSkillController.cs`, `BombData.asset`): Đặt bom tại vị trí player, nổ sau 2s, 50 ST gốc trong bán kính 5 unit, CD 12s.
  - **Kỹ năng 2 (Dash nổ)** (`ButtonDashExplosionSkill.cs`, `DashExplosionData.asset`): Lướt 3 unit trong 0.5s, nổ 15 ST gốc trong bán kính 3 unit, CD 6s.

- [x] **Kẻ địch (Enemies)**:
  - **Quái cận chiến** (`MeleeEnemyController.cs`, `MeleeEnemyAttack.cs`, `MeleeEnemySO.asset`): HP 220, Speed 3 unit/s. Tấn công góc nón 50°, tầm 1.3 unit, 30 ST gốc. AI tiếp cận -> tấn công -> đứng im 1s -> lặp lại.
  - **Quái đánh xa** (`RangedEnemyController.cs`, `RangedEnemyAttack.cs`, `RangedEnemySO.asset`): HP 180, Speed 2.7 unit/s, tầm 3 unit. Đạn độc bay max 5 unit, speed 10 unit/s. Độc 30 ST/s x 4 tick trong 3s (tick 0s, 1s, 2s, 3s). Reset thời gian khi dính đạn độc lại (`PlayerPoisonHandler.cs`).

- [x] **Wave, EXP & Level Up**:
  - **Wave System** (`WaveSpawnManager.cs`): Spawn ngẫu nhiên 3–4 quái cận chiến + 1–2 quái đánh xa mỗi wave. Chỉ spawn wave tiếp theo khi đã clear toàn bộ quái wave hiện tại.
  - **EXP & Level Up** (`PlayerLevel.cs`, `PlayerLevelGrowthData.asset`): +30 EXP mỗi quái. Đủ 100 EXP lên 1 cấp (giữ lại EXP dư).
  - **Thưởng lên cấp**: +40 Máu hiện tại & tối đa, +2 Giáp, +0.1 Damage Multiplier mỗi cấp.

- [x] **Giao diện (UI)**:
  - Canvas UI Overlay: Thanh máu Player (`PlayerHealthBar.cs`), Level hiện tại (`PlayerLevel.cs`).
  - Virtual Joystick di chuyển (`UIJoystick.cs`).
  - Nút kỹ năng có Text đếm ngược Cooldown (`ButtonBombSkill.cs`, `ButtonDashExplosionSkill.cs`).
  - WorldSpace HP Bar đính trên đầu từng con quái (`MeleeEnemyBarCanvas.cs`, `RangedBarCanvas.cs`).

---

### TIÊU CHÍ KỸ THUẬT & TỔ CHỨC CODE (TECHNICAL ARCHITECTURE)

- **Input System hiện đại (New Input System)**: Sử dụng `UnityEngine.InputSystem` cho di chuyển (Composite 2DVector phím WASD / Mũi tên) và phím tắt kỹ năng, tương thích hoàn toàn khi thiết lập *Active Input Handling: Input System Package (New)*.
- **Config tách biệt (ScriptableObjects)**: Toàn bộ chỉ số nằm trong thư mục `Assets/Resources/` (`EntitySO`, `ShotData`, `BombData`, `DashExplosionData`, `PlayerLevelGrowthData`), không hardcode trong code.
- **Tối ưu hiệu năng (Object Pooling)**: Sử dụng hệ thống `BasePooling` cho Đạn (`SpawnBullet`), Bom (`SpawnBomb`), Đạn độc (`SpawnProjectile`), Quái cận chiến (`SpawnMeleeEnemy`), Quái đánh xa (`SpawnRangedEnemy`).
- **Kiến trúc Code Clean**: Áp dụng mô hình `LoadMonoBehaviour` kế thừa, phân tách trách nhiệm rõ ràng (`Controller`, `Moving`, `Attack`, `DamageReceiver`, `DamageSender`).

---


## 📁 5. Cấu trúc thư mục chính (Folder Structure)

```text
Assets/
 ├── Resources/            # ScriptableObjects config (EntitySO, ShotData, BombData, LevelGrowth)
 └── _Data/
      ├── Player/          # Player Controller, Moving, Shooting, Level, HP Bar, Poison Handler
      ├── Enemy/           # Melee & Ranged Enemy Controllers, AI Movement & Attack
      ├── Bullet/          # Bullet Controller & Movement (Pooling)
      ├── Bomb/            # Bomb Skill Controller & Animation Events (Pooling)
      ├── Projectile/      # Poison Projectile (Ranged Enemy Pooling)
      ├── Canvas/          # UI Joystick, Skill Buttons, Cooldown Text
      └── _Script/         # Base Pooling, Damage System, Wave Spawn Manager
```
