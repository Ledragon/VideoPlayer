import { Video } from '../../models/Video';

interface PropType {
  video: Video;
}

const FileInfo = (props: PropType) => {
  const { video } = props;
  return <div></div>;
};

export default FileInfo;
