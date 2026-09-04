# 🛡️ Survival Top-Down Prototype

[![Unity](https://img.shields.io/badge/Unity-2022.3%2B-black?logo=unity)](https://unity.com/)
[![Language](https://img.shields.io/badge/Language-C%23-blue?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20PC-brightgreen)]()
[![Status](https://img.shields.io/badge/Status-Playable%20Prototype-orange)]()

Một dự án game hành động sinh tồn góc nhìn từ trên xuống (Top-Down Action Survival) được phát triển bằng Unity và C#. Dự án tập trung vào trải nghiệm chiến đấu nhịp độ nhanh, cơ chế điều khiển mượt mà trên cả Mobile lẫn PC, đồng thời áp dụng các Design Pattern chuẩn chỉnh để tối ưu hiệu năng và khả năng mở rộng.

---

## 🎮 Gameplay & Tính Năng Nổi Bật

### Cơ Chế Bắn & Tự Động Ngắm (Auto-Aim)
- **Hệ thống bắn 3 tia (Triple Shot):** Bắn chùm đạn góc nón (-15°, 0°, +15°) tạo cảm giác bắn đã tay và quét quái diện rộng.
- **Auto-Aim thông minh:** Tự động phát hiện và xoay nhân vật về phía mục tiêu gần nhất trong góc nón 90° phía trước mặt. Nếu kẻ địch ở phía sau, hệ thống tự động fallback quét 360° để không bỏ lỡ mục tiêu.
- **Cơ chế nạp đạn (Charge System):** Giới hạn số lượt bắn liên tục (tối đa 3 Charge) và tự động hồi phục theo thời gian để cân bằng nhịp độ combat.

### Bộ Kỹ Năng Chiến Thuật
- **Đặt bom hẹn giờ (Bomb Drop - Phím K):** Thả một quả bom tại vị trí hiện tại, phát nổ sau 2 giây gây sát thương diện rộng (AoE) cực lớn cho bầy quái đang bám đuôi.
- **Lướt nổ cơ động (Dash Explosion - Phím L / Shift):** Lướt nhanh về phía trước để né đòn nguy cấp, đồng thời tạo ra sóng nổ gây sát thương xung quanh điểm lướt.

### Kẻ Địch Đa Dạng & AI FSM
- **Quái cận chiến (Melee Enemy):** Tốc độ nhanh, tự động định vị và áp sát người chơi. Đòn chém có hitbox chính xác thông qua Animation Event.
- **Quái tầm xa (Ranged Enemy):** Giữ cự ly an toàn và bắn các cầu độc đạn đạo.
- **Hiệu ứng độc rút máu (Poison DoT):** Khi trúng đạn độc, người chơi sẽ bị trừ máu ngắt quãng theo từng nhịp (tick), thời gian độc được làm mới nếu tiếp tục dính đòn.

### Wave System & Tiến Trình Tăng Cấp (Level Up)
- **Spawn theo đợt:** Quái xuất hiện ngẫu nhiên theo wave tại các điểm spawn quanh bản đồ. Khi dọn sạch toàn bộ quái trong wave, đợt quái tiếp theo sẽ được kích hoạt.
- **EXP & Thăng cấp:** Tiêu diệt quái nhận EXP. Khi lên cấp, nhân vật được hồi máu, tăng HP tối đa, tăng chỉ số giáp (giảm sát thương nhận vào) và tăng hệ số sát thương đầu ra.

---

## 🕹️ Điều Khiển (Controls)

Dự án hỗ trợ mượt mà cả **Bàn phím PC** lẫn **Màn hình cảm ứng Mobile**:

| Thao tác | Phím trên PC | Thao tác trên Mobile |
| :--- | :--- | :--- |
| **Di chuyển** | `WASD` hoặc `Phím Mũi Tên` | **Virtual Joystick** kéo đa hướng |
| **Bắn thường (Triple Shot)** | `J` hoặc `Space` | Nút **Attack** |
| **Kỹ năng: Đặt bom** | `K` | Nút **Bomb Skill** (Có vòng đếm hồi chiêu) |
| **Kỹ năng: Lướt nổ** | `L` hoặc `Left Shift` | Nút **Dash Skill** (Có chỉ báo phạm vi) |

---

## Kiến Trúc & Kỹ Thuật (Under the Hood)

Dự án được xây dựng với mục tiêu code sạch, tách bạch trách nhiệm và tối ưu hóa tài nguyên cho thiết bị di động:

- **Finite State Machine (FSM):** Quản lý toàn bộ trạng thái của Player (`Idle`, `Move`, `Shot`) và Enemy (`Idle`, `Move`, `Attack`, `Dead`) giúp logic dễ bảo trì, không bị lồng chéo `if-else`.
- **Object Pooling (`BasePooling`):** Toàn bộ đạn, bom, cầu độc, quái và hiệu ứng VFX đều được tái sử dụng GameObject, loại bỏ hoàn toàn hiện tượng tụt FPS do `Instantiate` / `Destroy` liên tục.
- **Observer Pattern:** Tách rời logic máu và nạp đạn khỏi UI, tự động cập nhật thanh HP và Charge Bar mỗi khi dữ liệu thay đổi.
- **Data-Driven (ScriptableObjects):** Tất cả chỉ số (Máu, Giáp, Tốc độ, Sát thương, Mốc tăng trưởng level, Dữ liệu kỹ năng) đều được đóng gói trong các file ScriptableObject tại `Assets/Resources/`, dễ dàng cân bằng game mà không cần can thiệp code.
- **Unity New Input System:** Bắt sự kiện bàn phím và cảm ứng chuẩn xác, tương thích tốt với cấu hình Unity hiện đại.
- **WorldSpace UI Tracking:** Thanh máu trên đầu quái và người chơi tự động xoay hướng (`FaceCamera`), đảm bảo góc nhìn rõ ràng nhất trong không gian Top-Down.

---

## 📁 Cấu Trúc Thư Mục

```text
Assets/
├── Resources/              # ScriptableObjects cấu hình chỉ số (PlayerSO, ShotData, BombData...)
└── _Data/
     ├── Player/            # Logic Player (Controller, FSM States, Shooting, Level, Poison)
     ├── Enemy/             # Logic Quái (Melee & Ranged Controllers, AI States, Hitbox)
     ├── Bullet/            # Đạn thường của Player & Pooling
     ├── Bomb/              # Kỹ năng Đặt bom, Animation Event & AoE Damage
     ├── Projectile/        # Đạn độc đạn đạo của Quái tầm xa
     ├── Canvas/            # Toàn bộ UI (Joystick, Skill Buttons, Cooldown Text, Health Bar)
     ├── VFX/               # Hiệu ứng nổ, tia đạn & hạt particle
     └── _Script/           # Base Pooling, Damage System, Wave Spawn Manager
```

---

## Cách Cài Đặt & Chạy Game

### 1. Mở trên Unity Editor
1. Clone repo về máy:
   ```bash
   git clone https://github.com/DinhTanThanh/Survival-Top-down.git
   ```
2. Mở **Unity Hub** -> Nhấn **Add** -> Chọn thư mục dự án vừa tải.
3. Mở scene chính: `Assets/Scenes/SampleScene.unity`.
4. Nhấn nút **Play** trên Editor để trải nghiệm ngay.

### 2. Trải nghiệm trên Android (APK)
- Dự án có sẵn file build `SurvivalTopdown.apk` tại thư mục gốc. Bạn có thể chép trực tiếp vào điện thoại Android và cài đặt để test.

---

## 🗺️ Dự Định Phát Triển (Roadmap)
- [ ] Thêm hệ thống Boss xuất hiện sau mỗi 5 wave.
- [ ] Hệ thống rơi vật phẩm (Hồi máu, Tăng tốc độ đánh, Khiên hộ thể).
- [ ] Tích hợp Sound Effects (SFX) và Background Music (BGM).
- [ ] Bổ sung Shop nâng cấp chỉ số vĩnh viễn sau mỗi màn chơi.

---

## 👨‍💻 Tác Giả

- **Đinh Tấn Thành** - [GitHub Profile](https://github.com/DinhTanThanh)
- Dự án: [Survival-Top-down](https://github.com/DinhTanThanh/Survival-Top-down)
