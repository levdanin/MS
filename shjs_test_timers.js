console.log("Main window:", window.guid);
console.log("frame 0 window:", window.frames[0].contentWindow.guid);
console.log("frame 1 window:", window.frames[1].contentWindow.guid);

console.log("Timers:");
console.log("Main:", window.$timers.length);
console.log("frame 0:", window.frames[0].contentWindow.$timers.length);
console.log("frame 1:", window.frames[1].contentWindow.$timers.length);

evalcx("setTimeout(function(){console.info('timeout informing from ' + window.guid)})", window.frames[0].contentWindow);

console.log("Timers:");
console.log("Main:", window.$timers.length);
console.log("frame 0:", window.frames[0].contentWindow.$timers.length);
console.log("frame 1:", window.frames[1].contentWindow.$timers.length);


console.log("frame 0:", window.frames[0].contentWindow.$timers.toString());
console.log("frame 0:", window.frames[0].contentWindow.$timers[0].at);
console.log(Date.now())

console.log((window.frames[0].contentWindow.LAST_TIMER_EXECUTED &&
             window.frames[0].contentWindow.LAST_TIMER_EXECUTED + 1000 < Date.now()) || 
             window.frames[0].contentWindow.TIMER_LOOP_RUNNING);
     
evalcx("__wait();", window.frames[0].contentWindow);



evalcx("window.__wait();", window.frames[0].contentWindow);
evalcx("window.setTimeout(function(){console.info('timeout informing from ' + window.guid)}, 10)", window.frames[0].contentWindow);
console.log("frame 0:", window.frames[0].contentWindow.$timers[0].fn.fn.toString());
console.log("frame 0:", window.frames[0].contentWindow.$timers.length);
console.log("top:", window.$timers.length);