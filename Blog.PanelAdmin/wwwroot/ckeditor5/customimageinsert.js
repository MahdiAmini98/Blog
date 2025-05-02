
const imageIcon = '<svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="#f90000" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><rect x="3" y="3" width="18" height="18" rx="2"/><circle cx="8.5" cy="8.5" r="1.5"/><path d="M20.4 14.5L16 10 4 20"/></svg>';


import { ButtonView } from './ckeditor5.js';

export default class CustomImageInsert {

    static get pluginName() {
        return 'CustomImageInsert';
    }

    constructor(editor) {
        this.editor = editor;
    }

    init() {
        const editor = this.editor;

        editor.ui.componentFactory.add('insertImageFromLibrary', locale => {

            const view = new ButtonView(locale);

            view.set({
                label: 'درج تصویر از سرور خودمان',
                icon: imageIcon,
                tooltip: true
            });

            view.on('execute', () => {

                if (window.dotNetRefGlobal) {

                    window.dotNetRefGlobal.invokeMethodAsync('OpenImageDialogFromJS');
                }
            });

            return view;

        })
    }
}