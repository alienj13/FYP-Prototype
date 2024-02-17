
mergeInto(LibraryManager.library, {
  PromptFileUpload: function () {
    var input = document.createElement('input');
    input.type = 'file';
    input.accept = '.iqr';
    input.onchange = function (e) {
      var file = e.target.files[0];
      if (!file) {
        return;
      }
      var reader = new FileReader();
      reader.onload = function (ev) {
        var contents = ev.target.result;
        // Make sure the GameObject name and method name match exactly.
        SendMessage('FileHandler', 'ReceiveFileContent', contents);
      };
      reader.readAsText(file);
    };
    input.click();
  }
});


