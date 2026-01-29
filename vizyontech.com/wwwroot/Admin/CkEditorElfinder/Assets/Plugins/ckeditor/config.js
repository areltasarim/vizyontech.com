/**
 * @license Copyright (c) 2003-2021, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	config.language = 'tr';
	

	// Simplify the dialog windows.
	config.removeDialogTabs = 'image:advanced;link:advanced';
	config.filebrowserImageUploadUrl = '/Admin/Elfinder/UploadImage';
	config.filebrowserBrowseUrl = '/admin/elfinder';
	config.removePlugins = 'easyimage,cloudservices';//Easyimage, cloudervices eklentisini kapatmak için bu kod satýrýný ekleyin



	config.enterMode = CKEDITOR.ENTER_BR;
	config.shiftEnterMode = CKEDITOR.ENTER_BR;

	config.entities = false;
	config.basicEntities = false;
	config.fillEmptyBlocks = false;
};
