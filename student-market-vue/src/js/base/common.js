
import HEnum from './enum.js';
const HCommon = {

    /**
     * Định dạng dữ liệu
     * Author: Nguyễn Văn Huy (05/03/2023)
     */
    formatData(col, data) {
        switch (col.formatType) {
            case "date":
                return this.formatDate(
                    data,
                    HEnum.DateFormat.TypeForTable
                );

            case "money":
                return this.formatMoney(data);
            case "gender":
                return this.formatGender(data);
            case "role":
                return this.formatRole(data);
            case "approved":
                return this.formatApproved(data);
            case "createdDate":
                return this.formatTime(data);
            case "property":
                return this.formatProperty(data);
            case "active-status":
                return this.formatActiveStatus(data);
            default:
                return data;
        }
    },

    formatTime(CreatedDate) {
        CreatedDate = new Date(CreatedDate);
        let nowDate = new Date();
        let diff = nowDate - CreatedDate;
        let diffInDays = Math.floor(diff / (1000 * 60 * 60 * 24));

        if (diffInDays >= 365) {
            let diffInYears = Math.floor(diffInDays / 365);
            return diffInYears + " năm trước";
        }

        if (diffInDays >= 1) {
            return diffInDays + " ngày trước";
        }

        let diffInHours = Math.floor(diff / (1000 * 60 * 60));
        if (diffInHours >= 1) {
            return diffInHours + " giờ trước";
        }

        let diffInMinutes = Math.floor(diff / (1000 * 60));

        if (diffInMinutes < 1) {
            return "Vừa xong";
        }
        return diffInMinutes + " phút trước";
    },
    /**
     * Định dạng dữ liệu kiểu Date
     * @param {*} value biến date
     * Author: huynv (02/03/2023)
     */
    formatDate(value, type) {
        if (value) {
            let date = new Date(value);
            //Lấy ra ngày
            let day = date.getDate().toString().padStart(2, "0");
            //Lấy ra tháng
            let month = (date.getMonth() + 1).toString().padStart(2, "0");
            //Lấy ra năm
            let year = date.getFullYear();
            //Tạo thẻ td
            //Gán dữ liệu đã định dạng
            switch (type) {
                case 1:
                    return `${day}/${month}/${year}`
                default:
                    return `${year}-${month}-${day}`;
            }
        }
        return null;
    },
    /**
     * Định dạng dữ liệu kiểu Date
     * @param {*} value biến date
     * Author: huynv (02/03/2023)
     */
    formatNotify(notify) {
        if (notify.Content && notify.Content.includes("@FullName")) {
            return notify.Content.replace("@FullName", `<b>${notify.FullName}</b>`)
        }
        return notify.Content;
    },
    /**
     * Chuyển dữ liệu kiểu tiền tệ
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    formatMoney(value) {
        if (value) {
            // //Gán dữ liệu đã định dạng
            // value = value.toLocaleString("vi-VN", {
            //   style: "currency",
            //   currency: "VND",
            // });
            value = new Intl.NumberFormat("vi-VN", {
                style: "currency",
                currency: "VND",
            })
                .format(value)
                .replace("₫", "VNĐ");
            //Căn phải cho ô ngày
        }
        return value;
    },
    /**
     * Chuyển dữ liệu kiểu giới tính
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    formatGender(value) {
        value = Number.parseInt(value);
        if (value != null) {
            //Gán dữ liệu đã định dạng
            switch (value) {
                case 0:
                    return "Nữ";
                case 1:
                    return "Nam";
                case 2:
                    return "Khác";
            }
            //Căn phải cho ô ngày
        }
        return null;
    },

    /**
     * Chuyển dữ liệu kiểu vai trò
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    formatRole(value) {
        value = Number.parseInt(value);
        if (value != null) {
            //Gán dữ liệu đã định dạng
            switch (value) {
                case 0:
                    return "Người dùng";
                case 1:
                    return "Kiểm duyệt";
                case 2:
                    return "Admin";
            }
            //Căn phải cho ô ngày
        }
        return null;
    },
    /**
     * Chuyển dữ liệu kiểu trạng thái duyệt
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    formatApproved(value) {
        value = Number.parseInt(value);
        if (value != null) {
            //Gán dữ liệu đã định dạng
            switch (value) {
                case 0:
                    return "Chưa duyệt";
                case 1:
                    return "Đã duyệt";
                case 2:
                    return "Từ chối";
            }
            //Căn phải cho ô ngày
        }
        return null;
    },
    /**
     * Chuyển dữ liệu kiểu tính chất
     * Author: Nguyễn Văn Huy (19/04/2023)
     */
    formatProperty(value) {
        value = Number.parseInt(value);
        if (value != null) {
            //Gán dữ liệu đã định dạng
            switch (value) {
                case 0:
                    return "Dư nợ";
                case 1:
                    return "Dư có";
                case 2:
                    return "Không có dư";
            }
            //Căn phải cho ô ngày
        }
        return null;
    },
    /**
     * Chuyển dữ liệu kiểu trạng thái hoạt động
     * Author: Nguyễn Văn Huy (19/04/2023)
     */
    formatActiveStatus(value) {
        value = Number.parseInt(value);
        if (value != null) {
            //Gán dữ liệu đã định dạng
            switch (value) {
                case 0:
                    return "Đang sử dụng";
                case 1:
                    return "Ngừng sử dụng";
            }
            //Căn phải cho ô ngày
        }
        return null;
    },

    OptionsOperator: [
        { text: "Bằng", value: "equal" },
        { text: "Chứa", value: "contains" },
        { text: "Bắt đầu với", value: "startWith" },
        { text: "Kết thúc với", value: "endWith" },
        { text: "Trống", value: "isNull" },
        { text: "Không Trống", value: "isNotNull" },
        { text: "Nhỏ hơn", value: "smaller" },
        { text: "Nhỏ hơn hoặc bằng", value: "smallerOrEqual" },
        { text: "Lớn hơn", value: "bigger" },
        { text: "Lớn hơn hoặc bằng", value: "biggerOrEqual" },
        { text: "(Trống)", value: "isNull" },
        { text: "(Không Trống)", value: "isNotNull" },
    ],
    formatFilterText: function (condition) {
        var text = "";
        this.OptionsOperator.forEach((option) => {
            if (option.value == condition.Operator) {
                let value = (condition.Field == 'Gender') ? HCommon.formatGender(condition.Value) : condition.Value;
                if (value) {
                    text = `${condition.Title} ${option.text} "${value}"`;
                } else {
                    text = `${condition.Title} ${option.text}`;
                }
            }
        });
        return text;
    },


    /**
     * Tạo class căn chỉnh dữ liệu
     * @param {*} dataAlign
     * Author: Nguyễn Văn Huy(04/04/2023)
     */
    textAlign(dataAlign) {
        if (dataAlign) {
            return "text-align-" + dataAlign;
        } else return "text-align-left";
    },

    convertCamelToPascal(obj) {
        return Object.keys(obj).reduce((acc, key) => {
            acc[key.charAt(0).toUpperCase() + key.slice(1)] = obj[key];
            return acc;
        }, {});
    }
}
export default HCommon