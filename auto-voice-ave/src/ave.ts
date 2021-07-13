import * as vscode from 'vscode';
import "./configuration-provider";
import { ConfigurationProvider } from './configuration-provider';
import { WebEndpoint } from './webEndpoint';

export class Ave {

    public toggleListenCommand = "auto-voice-ave.listen";
    public activateCommand = "auto-voice-ave.activate";
    private listenMode = true;
    private isStartup = true;
    private statusbar: vscode.StatusBarItem;
    private configProvider: ConfigurationProvider;
    private webEndpoint: WebEndpoint;

    constructor() {
        this.configProvider = new ConfigurationProvider();
        this.statusbar = this.createStatusBarIcon();
        this.webEndpoint = new WebEndpoint("http://localhost:5000/ave-server/");
    }

    public onInitialize(context: vscode.ExtensionContext) {

        this.listenMode = this.configProvider.defaultListen();

        context.subscriptions.push(this.statusbar);
        context.subscriptions.push(this.registerListenCommand());
    }

    public onRun() {
        this.triggerListener();
        this.statusbar.show();
        this.isStartup = false;
    }

    public registerListenCommand(): vscode.Disposable {
        return vscode.commands.registerCommand(this.toggleListenCommand, () => {
            this.listenMode = !this.listenMode;
            this.triggerListener();
        });
    }

    private triggerListener(){
        if (this.listenMode) {
            this.showNotification("AVE enters listen mode!", false);
            this.statusbar.text = labels.statusListenOn;
            this.webEndpoint.startListening();

        } else {
            this.showNotification("AVE stopped listen mode.", false);
            this.statusbar.text = labels.statusListenOff;
            this.webEndpoint.stopListening();
        }
    }

    public createStatusBarIcon() {
        let statusbar = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Right, undefined);
        statusbar.name = "ave";
        statusbar.tooltip = "Automatic Voice Extension";
        statusbar.accessibilityInformation = { label: "AVE Automatic Voice Extension" };
        statusbar.command = this.toggleListenCommand;
        return statusbar;
    }

    private showNotification(message: string, showDuringStartup: boolean = true) {
        if ((showDuringStartup || !this.isStartup)
            && this.configProvider.showNotifications()) {
            vscode.window.showInformationMessage(message);
        }
    }
}

let labels = {
    statusListenOn: '$(hubot)  $(record~spin)AVE $(debug-pause)',
    statusListenOff: '$(hubot) AVE $(run)',
};