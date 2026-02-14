mergeInto(LibraryManager.library, {
    OpenMapFilePicker: function() {
        var input = document.createElement('input');
        input.type = 'file';
        input.accept = '.json,application/json';

        input.onchange = function(event) {
            var file = event.target.files[0];
            if (!file) {
                return;
            }

            var reader = new FileReader();
            reader.onload = function(e) {
                var contents = e.target.result;

                try {
                    // Assumes the GameObject with MapLoadManager is named "MapLoadManager".
                    if (typeof SendMessage === 'function') {
                        SendMessage('MapLoadManager', 'OnMapFileSelected', contents);
                    } else if (typeof unityInstance !== 'undefined' && unityInstance != null && typeof unityInstance.SendMessage === 'function') {
                        unityInstance.SendMessage('MapLoadManager', 'OnMapFileSelected', contents);
                    } else {
                        console.error('Unity SendMessage/unityInstance not found. Cannot deliver file contents to MapLoadManager.');
                    }
                } catch (err) {
                    console.error('Error sending map JSON to Unity:', err);
                }
            };

            reader.readAsText(file);
        };

        // Trigger the browser file picker
        input.click();
    }
});
