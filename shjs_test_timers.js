console.log("Main window:", window.guid);
console.log("frame 0 window:", window.frames[0].contentWindow.guid);
console.log("frame 1 window:", window.frames[1].contentWindow.guid);

console.log("Timers:");
console.log("Main:", window.$timers.length);
console.log("frame 0:", window.frames[0].contentWindow.$timers.length);
console.log("frame 1:", window.frames[1].contentWindow.$timers.length);

evalcx("setTimeout(function(){print('timeout printing from ' + window.guid); console.info('timeout informing from ' + window.guid)})", window.frames[0].contentWindow);

console.log("Timers:");
console.log("Main:", window.$timers.length);
console.log("frame 0:", window.frames[0].contentWindow.$timers.length);
console.log("frame 1:", window.frames[1].contentWindow.$timers.length);
