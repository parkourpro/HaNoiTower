using UnityEngine;
using UnityEngine.UI;
using TMPro; // Cần thêm thư viện TextMeshPro

public class SliderValueDisplay : MonoBehaviour
{
    public Slider slider; // Tham chiếu đến slider
    public TextMeshProUGUI sliderValueText; // Tham chiếu đến TextMeshPro để hiển thị giá trị

    void Start()
    {
        // Đăng ký hàm OnSliderValueChanged để cập nhật giá trị khi slider thay đổi
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        // Gọi hàm để cập nhật giá trị lúc bắt đầu
        OnSliderValueChanged(slider.value);
    }

    void OnSliderValueChanged(float value)
    {
        // Cập nhật giá trị hiển thị cho TextMeshPro
        sliderValueText.text = value.ToString("0.00"); // Định dạng thành chuỗi với 2 số thập phân
    }
}
