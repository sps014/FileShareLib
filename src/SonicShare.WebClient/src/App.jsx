import React, { useEffect, useState } from "react";
import { BrowserRouter, Routes, Route, Link, useParams } from "react-router-dom";

const GlassCard = ({ file, children }) => {

    const iconClass = getIconByExtension(file.name);

  return (

    <div
      style={{
        backdropFilter: "blur(10px)",
        backgroundColor: "rgba(255, 255, 255, 0.15)",
        borderRadius: "15px",
        border: "1px solid rgba(255, 255, 255, 0.3)",
        boxShadow: "0 4px 30px rgba(0, 0, 0, 0.1)",
        padding: "10px 15px",
        color: "#fff",
        fontFamily: "Segoe UI, Tahoma, Geneva, Verdana, sans-serif",
        maxWidth: "900px",
        width: "95%",
        display: "flex",
        alignItems: "center",
        justifyContent: "space-between",
        marginBottom: "15px",
      }}
    >
       {/* Icon section */}
       <div style={{ marginRight: "15px", fontSize: "2rem", flexShrink: 0 }}>
        <i className={iconClass}></i>
      </div>
      <div style={{ flex: 1, paddingRight: 10 }}>
        <h3 style={{ margin: "0 0 5px", fontSize: "1rem", wordBreak: "break-word" }}>
          {file.name}
        </h3>
        <p style={{ fontSize: "0.8rem", margin: "0", wordBreak: "break-word" }}>
          <strong>Hash:</strong> {file.hash}
        </p>
        <p style={{ fontSize: "0.8rem", margin: "0" }}>
          <strong>Content Type:</strong> {file.contentType}
        </p>
      </div>
      <div style={{ display: "flex", gap: "10px" }}>{children}</div>
    </div>
  );
};

const getIconByExtension = (filename) => {
  const ext = filename.split('.').pop().toLowerCase();

const map = {
    pdf: "fa-solid fa-file-pdf",
    doc: "fa-solid fa-file-word",
    docx: "fa-solid fa-file-word",
    xls: "fa-solid fa-file-excel",
    xlsx: "fa-solid fa-file-excel",
    ppt: "fa-solid fa-file-powerpoint",
    pptx: "fa-solid fa-file-powerpoint",
    mp4: "fa-solid fa-file-video",
    mkv: "fa-solid fa-file-video",
    webm: "fa-solid fa-file-video",
    mp3: "fa-solid fa-file-audio",
    wav: "fa-solid fa-file-audio",
    png: "fa-solid fa-file-image",
    jpg: "fa-solid fa-file-image",
    jpeg: "fa-solid fa-file-image",
    gif: "fa-solid fa-file-image",
    zip: "fa-solid fa-file-archive",
    rar: "fa-solid fa-file-archive",
    txt: "fa-solid fa-file-alt",
    json: "fa-solid fa-file-code",
    js: "fa-solid fa-file-code",
    html: "fa-solid fa-file-code",
    css: "fa-solid fa-file-code",
  };

  return map[ext] || "fa-solid fa-file";
}

const containerStyle = {
  minHeight: "100vh",
  width: "98vw",  // use 100%, not 100vw to avoid scrollbar issues
  backgroundColor: "rgba(30, 30, 30, 0.85)",
  display: "flex",
  flexDirection: "column",
  justifyContent: "flex-start",
  alignItems: "center",
  paddingTop: "30px",
  paddingBottom: "30px",
  boxSizing: "border-box",
  overflowX: "hidden", // prevent horizontal scroll
};



const buttonStyle = {
  backgroundColor: "rgba(255, 255, 255, 0.25)",
  border: "1px solid rgba(255, 255, 255, 0.4)",
  borderRadius: "8px",
  padding: "7px 15px",
  cursor: "pointer",
  color: "white",
  fontWeight: "600",
  backdropFilter: "blur(5px)",
  transition: "background-color 0.3s ease",
};

function FileList() {
  const [files, setFiles] = React.useState([]);
  const [loading, setLoading] = React.useState(true);
  const [error, setError] = React.useState(null);

  React.useEffect(() => {
    fetch("http://localhost:5001/api/shared/getAll")
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch files");
        return res.json();
      })
      .then((data) => {
        setFiles(data);
        setLoading(false);
      })
      .catch((err) => {
        setError(err.message);
        setLoading(false);
      });
  }, []);

  const isPlayable = (file) =>
    file.contentType.startsWith("video/") || file.contentType.startsWith("audio/");

  if (loading)
    return <p style={{ textAlign: "center", color: "white" }}>Loading files...</p>;
  if (error)
    return <p style={{ textAlign: "center", color: "red" }}>Error: {error}</p>;

  return (
    <div style={containerStyle}>
      <h2 style={{ color: "white", marginBottom: 20 }}>Files - SonicShare</h2>
      {files.map((file) => (
        <GlassCard key={file.hash} file={file}>
          {isPlayable(file) && (
            <a href={`/play/${file.hash}`} style={{ textDecoration: "none" }}>
              <button style={buttonStyle}>Play</button>
            </a>
          )}
          <a
            href={`http://localhost:5001/api/shared/download?hash=${file.hash}`}
            download={file.name}
            target="_blank"
            rel="noopener noreferrer"
          >
            <button style={buttonStyle}>Download</button>
          </a>
        </GlassCard>
      ))}
    </div>
  );
}



// Video player page
function VideoPlayerPage() {
  const { hash } = useParams();
  const videoUrl = `http://localhost:5001/api/shared/download?hash=${hash}`;

  return (
    <div style={{ padding: 20, color: 'white', textAlign: 'center' }}>
      <h2>Video Player</h2>
      <video
        src={videoUrl}
        controls
        autoPlay
        style={{ maxWidth: "90vw", maxHeight: "60vh", marginTop: 20, borderRadius: 10 }}
      />
      <br />
      <Link to="/" style={{ color: '#61dafb', marginTop: 20, display: 'inline-block' }}>
        ← Back to list
      </Link>
    </div>
  );
}



export default function App() {
  return (
    <div
      style={{
        minHeight: "100vh",
        background:
          "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
        padding: 20,
        fontFamily: "Segoe UI, Tahoma, Geneva, Verdana, sans-serif",
      }}
    >
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<FileList />} />
          <Route path="/play/:hash" element={<VideoPlayerPage />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}
