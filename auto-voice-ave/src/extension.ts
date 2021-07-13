// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import {Ave} from './ave';



// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {
	
	console.log("AVE(Voice) activated.");

	// The command has been defined in the package.json file
	// Now provide the implementation of the command with registerCommand
	// The commandId parameter must match the command field in package.json
	let ave = new Ave();
	context.subscriptions.push(vscode.commands.registerCommand(ave.activateCommand, () => {
		let helloMessage = "AVE is now active.\nClick the statusbar icon to activate listening mode.";
		vscode.window.showInformationMessage(helloMessage);

		ave.onInitialize(context);
		ave.onRun();
	}));
}

// this method is called when your extension is deactivated
export function deactivate() { 
	console.log("AVE deactivated.");
}
