function SetLoadPageVisible(value) {
    if (value) {
        loadingCover.style.background = "url('Images/background.png') center / cover";
        loadingCover.style.display = "";
    } else {
        loadingCover.style.background = "";
        loadingCover.style.display = "none";
    }
}

function SetLoadPageProgress(value) {
    progressBarEmpty.style.display = "";
    const adjustedProgress = Math.max(value, 0.05);
    progressBarFull.style.width = `${100 * adjustedProgress}%`;
}
