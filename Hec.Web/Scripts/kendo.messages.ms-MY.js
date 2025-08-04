(function ($, undefined) {
/* FlatColorPicker messages */

if (kendo.ui.FlatColorPicker) {
kendo.ui.FlatColorPicker.prototype.options.messages =
$.extend(true, kendo.ui.FlatColorPicker.prototype.options.messages,{
  "apply": "Gunakan",
  "cancel": "Batal"
});
}

/* ColorPicker messages */

if (kendo.ui.ColorPicker) {
kendo.ui.ColorPicker.prototype.options.messages =
$.extend(true, kendo.ui.ColorPicker.prototype.options.messages,{
  "apply": "Gunakan",
  "cancel": "Batal"
});
}

/* ColumnMenu messages */

if (kendo.ui.ColumnMenu) {
kendo.ui.ColumnMenu.prototype.options.messages =
$.extend(true, kendo.ui.ColumnMenu.prototype.options.messages,{
  "sortAscending": "Susun Menaik",
  "sortDescending": "Susun Menurun",
  "filter": "Tapis",
  "columns": "Lajur",
  "done": "Selesai",
  "settings": "Tetapan Lajur",
  "lock": "Kunci",
  "unlock": "Buka"
});
}

/* Editor messages */

if (kendo.ui.Editor) {
kendo.ui.Editor.prototype.options.messages =
$.extend(true, kendo.ui.Editor.prototype.options.messages,{
  "bold": "Tebal",
  "italic": "Codong",
  "underline": "Bergaris",
  "strikethrough": "Potong",
  "superscript": "Superskrip",
  "subscript": "Subskrip",
  "justifyCenter": "Tengahkan teks",
  "justifyLeft": "Rapatkan teks ke kiri",
  "justifyRight": "Rapatkan teks ke kanan",
  "justifyFull": "Justikasikan teks",
  "insertUnorderedList": "Masukkan senarai tidak bernombor",
  "insertOrderedList": "Masukkan senarai bernombor",
  "indent": "Inden",
  "outdent": "Outden",
  "createLink": "Masukkan pautan",
  "unlink": "Buang pautan",
  "insertImage": "Masukkan imej",
  "insertHtml": "Masukkan HTML",
  "viewHtml": "Lihat HTML",
  "fontName": "Pilih famili font",
  "fontNameInherit": "(font diwarisi)",
  "fontSize": "Pilih saiz font",
  "fontSizeInherit": "(saiz diwarisi)",
  "formatBlock": "Format",
  "formatting": "Format",
  "foreColor": "Warna",
  "backColor": "Warna latar",
  "style": "Gaya",
  "emptyFolder": "Folder kosong",
  "uploadFile": "Muat naik",
  "orderBy": "Susun mengikut:",
  "orderBySize": "Saiz",
  "orderByName": "Nama",
  "invalidFileType": "Fail \"{0}\" yang dipilih adalah tidak sah. Jenis fail yang disokong adalah {1}.",
  "deleteFile": 'Ada yakin untuk hapuskan "{0}"?',
  "overwriteFile": 'Fail bernama "{0}" sudah wujud dalam direktori semasa. Anda ingin gantikan ia?',
  "directoryNotFound": "Direktori dengan nama ini tidak ditemui.",
  "imageWebAddress": "Alamat web",
  "imageAltText": "Teks alternatif",
  "imageWidth": "Lebar (px)",
  "imageHeight": "Tinggi (px)",
  "fileWebAddress": "Alamat web",
  "fileTitle": "Tajuk",
  "linkWebAddress": "Alamat web",
  "linkText": "Teks",
  "linkToolTip": "ToolTip",
  "linkOpenInNewWindow": "Buka pautan di tetingkap baru",
  "dialogUpdate": "Kemaskini",
  "dialogInsert": "Masukkan",
  "dialogButtonSeparator": "atau",
  "dialogCancel": "Batal",
  "createTable": "Bina jadual",
  "addColumnLeft": "Tambah lajur di kiri",
  "addColumnRight": "Tambah lajur di kanan",
  "addRowAbove": "Tambah baris di atas",
  "addRowBelow": "Tambah baris di bawah",
  "deleteRow": "Padam baris",
  "deleteColumn": "Padam lajur"
});
}

/* FileBrowser messages */

if (kendo.ui.FileBrowser) {
kendo.ui.FileBrowser.prototype.options.messages =
$.extend(true, kendo.ui.FileBrowser.prototype.options.messages,{
  "uploadFile": "Muat Naik",
  "orderBy": "Susun mengikut",
  "orderByName": "Nama",
  "orderBySize": "Saiz",
  "directoryNotFound": "Direktori dengan nama ini tidak ditemui.",
  "emptyFolder": "Folder kosong",
  "deleteFile": 'Anda yakin untuk hapuskan "{0}"?',
  "invalidFileType": "Fail \"{0}\" yang dipilih adalah tidak sah. Jenis fail yang disokong adalah {1}.",
  "overwriteFile": "Fail bernama \"{0}\" sudah wujud dalam direktori semasa. Anda ingin gantikan ia?",
  "dropFilesHere": "tarik fail ke sini untuk memuat naik",
  "search": "Cari"
});
}

/* FilterCell messages */

if (kendo.ui.FilterCell) {
kendo.ui.FilterCell.prototype.options.messages =
$.extend(true, kendo.ui.FilterCell.prototype.options.messages,{
  "isTrue": "adalah ya",
  "isFalse": "adalah tidak",
  "filter": "Tapis",
  "clear": "Padam",
  "operator": "Operator"
});
}

/* FilterCell operators */

if (kendo.ui.FilterCell) {
kendo.ui.FilterCell.prototype.options.operators =
$.extend(true, kendo.ui.FilterCell.prototype.options.operators,{
  "string": {
    "eq": "Sama dengan",
    "neq": "Tidak sama dengan",
    "startswith": "Dimulakan dengan",
    "contains": "Mengandungi",
    "doesnotcontain": "Tidak mengandungi",
    "endswith": "Diakhiri dengan"
  },
  "number": {
    "eq": "Sama dengan",
    "neq": "Tidak sama dengan",
    "gte": "Lebih atau sama dengan",
    "gt": "Lebih dari",
    "lte": "Kurang atau sama dengan",
    "lt": "Kurang dari"
  },
  "date": {
    "eq": "Sama dengan",
    "neq": "Tidak sama dengan",
    "gte": "Selepas atau sama dengan",
    "gt": "Selepas",
    "lte": "Sebelum atau sama dengan",
    "lt": "Sebelum"
  },
  "enums": {
    "eq": "Sama dengan",
    "neq": "Tidak sama dengan"
  }
});
}

/* FilterMenu messages */

if (kendo.ui.FilterMenu) {
kendo.ui.FilterMenu.prototype.options.messages =
$.extend(true, kendo.ui.FilterMenu.prototype.options.messages,{
  "info": "Paparkan item yang nilainya:",
  "isTrue": "adalah ya",
  "isFalse": "adalah tidak",
  "filter": "Tapis",
  "clear": "Padam",
  "and": "Dan",
  "or": "Atau",
  "selectValue": "-Pilih nilai-",
  "operator": "Operator",
  "value": "Nilai",
  "cancel": "Batal"
});
}

/* FilterMenu operator messages */

if (kendo.ui.FilterMenu) {
kendo.ui.FilterMenu.prototype.options.operators =
$.extend(true, kendo.ui.FilterMenu.prototype.options.operators,{
  "string": {
    "eq": "Sama dengan",
    "neq": "Tidak sama dengan",
    "startswith": "Dimulakan dengan",
    "contains": "Mengandungi",
    "doesnotcontain": "Tidak mengandungi",
    "endswith": "Diakhiri dengan"
  },
  "number": {
    "eq": "Sama dengan",
    "neq": "Tidak sama dengan",
    "gte": "Lebih atau sama dengan",
    "gt": "Lebih dari",
    "lte": "Kurang atau sama dengan",
    "lt": "Kurang dari"
  },
  "date": {
    "eq": "Sama dengan",
    "neq": "Tidak sama dengan",
    "gte": "Selepas atau sama dengan",
    "gt": "Selepas",
    "lte": "Sebelum atau sama dengan",
    "lt": "Sebelum"
  },
  "enums": {
    "eq": "Sama dengan",
    "neq": "Tidak sama dengan"
  }
});
}

/* FilterMultiCheck messages */

if (kendo.ui.FilterMultiCheck) {
kendo.ui.FilterMultiCheck.prototype.options.messages =
$.extend(true, kendo.ui.FilterMultiCheck.prototype.options.messages,{
  "checkAll": "Pilih Semua",
  "clear": "Padam",
  "filter": "Tapis"
});
}

/* Gantt messages */

if (kendo.ui.Gantt) {
kendo.ui.Gantt.prototype.options.messages =
$.extend(true, kendo.ui.Gantt.prototype.options.messages,{
  "actions": {
    "addChild": "Tambah Anak",
    "append": "Tambah Tugas",
    "insertAfter": "Tambah Di bawah",
    "insertBefore": "Tambah Di atas",
    "pdf": "Eksport ke PDF"
  },
  "cancel": "Batal",
  "deleteDependencyWindowTitle": "Buang pergantungan",
  "deleteTaskWindowTitle": "Buang tugas",
  "destroy": "Buang",
  "editor": {
    "assingButton": "Serahkan",
    "editorTitle": "Tugas",
    "end": "Tamat",
    "percentComplete": "Selesai",
    "resources": "Sumber",
    "resourcesEditorTitle": "Sumber",
    "resourcesHeader": "Sumber",
    "start": "Mula",
    "title": "Tajuk",
    "unitsHeader": "Unit"
  },
  "save": "Simpan",
  "views": {
    "day": "Hari",
    "end": "Tamat",
    "month": "Bulan",
    "start": "Mula",
    "week": "Minggu",
    "year": "Tahun"
  }
});
}

/* Grid messages */

if (kendo.ui.Grid) {
kendo.ui.Grid.prototype.options.messages =
$.extend(true, kendo.ui.Grid.prototype.options.messages,{
  "commands": {
    "cancel": "Batalkan perubahan",
    "canceledit": "Batal",
    "create": "Tambah rekod baru",
    "destroy": "Buang",
    "edit": "Edit",
    "excel": "Eksport ke Excel",
    "pdf": "Eksport ke PDF",
    "save": "Simpan perubahan",
    "select": "Pilih",
    "update": "Kemaskini"
  },
  "editable": {
    "cancelDelete": "Batal",
    "confirmation": "Anda yakin untuk buang rekod ini?",
    "confirmDelete": "Buang"
  },
  "noRecords": "Tiada rekod ditemui."
});
}

/* Groupable messages */

if (kendo.ui.Groupable) {
kendo.ui.Groupable.prototype.options.messages =
$.extend(true, kendo.ui.Groupable.prototype.options.messages,{
  "empty": "Tarik tajuk lajur dan lepaskan di sini untuk grup mengikut lajur tersebut"
});
}

/* NumericTextBox messages */

if (kendo.ui.NumericTextBox) {
kendo.ui.NumericTextBox.prototype.options =
$.extend(true, kendo.ui.NumericTextBox.prototype.options,{
  "upArrowText": "Naikkan nilai",
  "downArrowText": "Turunkan nilai"
});
}

/* Pager messages */

if (kendo.ui.Pager) {
kendo.ui.Pager.prototype.options.messages =
$.extend(true, kendo.ui.Pager.prototype.options.messages,{
  "allPages": "Semua",
  "display": "{0} - {1} dari {2} item",
  "empty": "Tiada item untuk dipaparkan",
  "page": "Halaman",
  "of": "dari {0}",
  "itemsPerPage": "item per halaman",
  "first": "Pergi ke halaman pertama",
  "previous": "Pergi ke halaman sebelumnya",
  "next": "Pergi ke halaman seterusnya",
  "last": "Pergi ke halaman terakhir",
  "refresh": "Muat semula",
  "morePages": "Halaman lain"
});
}

/* PivotGrid messages */

if (kendo.ui.PivotGrid) {
kendo.ui.PivotGrid.prototype.options.messages =
$.extend(true, kendo.ui.PivotGrid.prototype.options.messages,{
  "measureFields": "Tarik Medan Data Ke Sini",
  "columnFields": "Tarik Medan Lajur Ke Sini",
  "rowFields": "Tarik Medan Baris Ke Sini"
});
}

/* PivotFieldMenu messages */

if (kendo.ui.PivotFieldMenu) {
kendo.ui.PivotFieldMenu.prototype.options.messages =
$.extend(true, kendo.ui.PivotFieldMenu.prototype.options.messages,{
  "info": "Paparkan item yang nilainya:",
  "filterFields": "Tapis Medan",
  "filter": "Tapis",
  "include": "Sertakan Medan...",
  "title": "Medan untuk disertakan",
  "clear": "Padam",
  "ok": "Ok",
  "cancel": "Batal",
  "operators": {
    "contains": "Mengandungi",
    "doesnotcontain": "Tidak mengandungi",
    "startswith": "Dimulakan dengan",
    "endswith": "Diakhiri dengan",
    "eq": "Sama dengan",
    "neq": "Tidak sama dengan"
  }
});
}

/* RecurrenceEditor messages */

if (kendo.ui.RecurrenceEditor) {
kendo.ui.RecurrenceEditor.prototype.options.messages =
$.extend(true, kendo.ui.RecurrenceEditor.prototype.options.messages,{
  "frequencies": {
    "never": "Tidak pernah",
    "hourly": "Tiap jam",
    "daily": "Tiap hari",
    "weekly": "Tiap minggu",
    "monthly": "Tiap bulan",
    "yearly": "Tiap tahun"
  },
  "hourly": {
    "repeatEvery": "Ulang setiap: ",
    "interval": " jam"
  },
  "daily": {
    "repeatEvery": "Ulang setiap ",
    "interval": " hari"
  },
  "weekly": {
    "interval": " minggu",
    "repeatEvery": "Ulang setiap ",
    "repeatOn": "Ulang pada: "
  },
  "monthly": {
    "repeatEvery": "Ulang setiap ",
    "repeatOn": "Ulang pada: ",
    "interval": " bulan",
    "day": "Hari "
  },
  "yearly": {
    "repeatEvery": "Ulang setiap ",
    "repeatOn": "Ulang pada: ",
    "interval": " tahun",
    "of": " dari "
  },
  "end": {
    "label": "Tamat:",
    "mobileLabel": "Tamat",
    "never": "Tidak pernah",
    "after": "Lepas ",
    "occurrence": " kali",
    "on": "Pada "
  },
  "offsetPositions": {
    "first": "pertama",
    "second": "kedua",
    "third": "ketiga",
    "fourth": "keempat",
    "last": "terakhir"
  },
  "weekdays": {
    "day": "hari",
    "weekday": "hari biasa",
    "weekend": "hujung minggu"
  }
});
}

/* Scheduler messages */

if (kendo.ui.Scheduler) {
kendo.ui.Scheduler.prototype.options.messages =
$.extend(true, kendo.ui.Scheduler.prototype.options.messages,{
  "allDay": "sepanjang hari",
  "date": "Tarikh",
  "event": "Acara",
  "time": "Masa",
  "showFullDay": "Papar hari penuh",
  "showWorkDay": "Papar waktu bekerja",
  "today": "Hari Ini",
  "save": "Simpan",
  "cancel": "Batal",
  "destroy": "Buang",
  "deleteWindowTitle": "Buang acara",
  "ariaSlotLabel": "Dipilih dari {0:t} ke {1:t}",
  "ariaEventLabel": "{0} pada {1:D} {2:t}",
  "editable": {
    "confirmation": "Anda pasti ingin memadamkan acara ini?"
  },
  "views": {
    "day": "Hari",
    "week": "Minggu",
    "workWeek": "Minggu Bekerja",
    "agenda": "Agenda",
    "month": "Bulan"
  },
  "recurrenceMessages": {
    "deleteWindowTitle": "Padam Item Berulang",
    "deleteWindowOccurrence": "Padam ulangan ini",
    "deleteWindowSeries": "Padam semua ulangan",
    "editWindowTitle": "Edit Item Berulang",
    "editWindowOccurrence": "Edit ulangan ini",
    "editWindowSeries": "Edit semua ulangan",
    "deleteRecurring": "Anda ingin padamkan hanya ulangan acara ini atau kesemua ulangan?",
    "editRecurring": "Anda ingin edit hanya ulangan acara ini atau kesemua ulangan?"
  },
  "editor": {
    "title": "Tajuk",
    "start": "Mula",
    "end": "Tamat",
    "allDayEvent": "Acara sepanjang hari",
    "description": "Butiran",
    "repeat": "Ulang",
    "timezone": " ",
    "startTimezone": "Zon masa mula",
    "endTimezone": "Zon masa tamat",
    "separateTimezones": "Gunakan zon masa berbeza untuk mula dan tamat",
    "timezoneEditorTitle": "Zon masa",
    "timezoneEditorButton": "Zon masa",
    "timezoneTitle": "Zon masa",
    "noTimezone": "Tiada zon masa",
    "editorTitle": "Acara"
  }
});
}

/* Slider messages */

if (kendo.ui.Slider) {
kendo.ui.Slider.prototype.options =
$.extend(true, kendo.ui.Slider.prototype.options,{
  "increaseButtonTitle": "Tambahkan",
  "decreaseButtonTitle": "Kurangkan"
});
}

/* TreeList messages */

if (kendo.ui.TreeList) {
kendo.ui.TreeList.prototype.options.messages =
$.extend(true, kendo.ui.TreeList.prototype.options.messages,{
  "noRows": "Tiada rekod untuk dipaparkan",
  "loading": "Memuat turun...",
  "requestFailed": "Permintaan gagal.",
  "retry": "Cuba semula",
  "commands": {
      "edit": "Edit",
      "update": "Kemaskini",
      "canceledit": "Batal",
      "create": "Tambah rekod baru",
      "createchild": "Tambah rekod anak",
      "destroy": "Buang",
      "excel": "Eksport ke Excel",
      "pdf": "Eksport ke PDF"
  }
});
}

/* TreeView messages */

if (kendo.ui.TreeView) {
kendo.ui.TreeView.prototype.options.messages =
$.extend(true, kendo.ui.TreeView.prototype.options.messages,{
  "loading": "Memuat turun...",
  "requestFailed": "Permintaan gagal.",
  "retry": "Cuba semula"
});
}

/* Upload messages */

if (kendo.ui.Upload) {
kendo.ui.Upload.prototype.options.localization=
$.extend(true, kendo.ui.Upload.prototype.options.localization,{
  "select": "Pilih fail...",
  "cancel": "Batal",
  "retry": "Cuba semula",
  "remove": "Buang",
  "uploadSelectedFiles": "Muat naik fail",
  "dropFilesHere": "tarik fail ke sini untuk memuat naik",
  "statusUploading": "memuat naik",
  "statusUploaded": "telah dimuat naik",
  "statusWarning": "amaran",
  "statusFailed": "gagal",
  "headerStatusUploading": "Sedang memuat naik...",
  "headerStatusUploaded": "Selesai"
});
}

/* Validator messages */

if (kendo.ui.Validator) {
kendo.ui.Validator.prototype.options.messages =
$.extend(true, kendo.ui.Validator.prototype.options.messages,{
  "required": "{0} diperlukan",
  "pattern": "{0} tidak sah",
  "min": "{0} sepatutnya lebih besar atau sama dengan {1}",
  "max": "{0} sepatutnya lebih kecil atau sama dengan {1}",
  "step": "{0} tidak sah",
  "email": "{0} bukan alamat emel yang sah",
  "url": "{0} bukan alamat URL yang sah",
  "date": "{0} bukan tarikh yang sah",
  "dateCompare": "Tarikh tamat sepatutnya lebih kemudian dari tarikh mula"
});
}
})(window.kendo.jQuery);
