import { ActionIcon, Badge, Card, Group, Image, Overlay, Stack, Text, Tooltip } from '@mantine/core';
import { IconEye, IconEyeOff, IconInfoCircle } from '@tabler/icons-react';
import { timeFormat } from 'd3-time-format';
import { useMemo, useState } from 'react';

import { Video } from '../../models/Video';
import { useGetContactSheetForVideoQuery, useGetThumbnailsForVideoQuery } from '../../redux';
import AddToPlaylist from './AddToPlaylist';
import getBase64Url from './getBase64Url';

const tf = timeFormat('%Y-%m-%d');

interface PropType {
  showContactSheet?: boolean;
  video: Video;
  onInfoClick?: (video: Video) => void;
}

const VideoImageItem = ({ video, onInfoClick, showContactSheet }: PropType) => {
  const { data: thumbs } = useGetThumbnailsForVideoQuery(video.id);
  const [hovered, setHovered] = useState<boolean>(false);
  const { data: contactSheet } = useGetContactSheetForVideoQuery(video.id);

  const onInfoButtonClick = (e: React.MouseEvent) => {
    e.stopPropagation();
    if (onInfoClick) {
      onInfoClick(video);
    }
  };

  const handleMouseEnter = () => {
    setHovered(true);
  };

  const handleMouseLeave = () => {
    setHovered(false);
  };

  const image = useMemo(() => {
    if (!showContactSheet) {
      return thumbs?.length ? thumbs[0].image : undefined;
    } else {
      return contactSheet ? contactSheet.image : undefined;
    }
  }, [thumbs, showContactSheet, contactSheet]);

  const formattedDate = useMemo(() => {
    const date = new Date(video.dateAdded);
    return tf(date);
  }, [video]);

  return (
    <Card
      shadow="sm"
      padding={0}
      radius="md"
      withBorder
      style={{
        cursor: 'pointer',
        transition: 'transform 0.2s ease, box-shadow 0.2s ease',
        transform: hovered ? 'translateY(-4px)' : 'translateY(0)',
        boxShadow: hovered ? 'var(--mantine-shadow-md)' : 'var(--mantine-shadow-sm)'
      }}
      onMouseEnter={handleMouseEnter}
      onMouseLeave={handleMouseLeave}
      onClick={onInfoButtonClick}
      id={`video-${video.id}`}>
      <Card.Section style={{ position: 'relative' }}>
        <Image src={image ? getBase64Url(image) : '/no-image-14600.svg'} height={200} alt={video.title} fit="cover" fallbackSrc="/no-image-14600.svg" />

        {/* Duration Badge */}
        <Badge
          variant="filled"
          color="dark"
          size="sm"
          style={{
            position: 'absolute',
            bottom: 8,
            right: 8,
            backgroundColor: 'rgba(0, 0, 0, 0.8)',
            backdropFilter: 'blur(4px)'
          }}>
          {video.lengthString || video.length}
        </Badge>

        {/* Contact Sheet Status */}
        {!contactSheet && (
          <Tooltip label="No contact sheet available">
            <div style={{ position: 'absolute', top: 8, right: 8 }}>
              <IconEyeOff size={20} color="rgba(255, 255, 255, 0.8)" />
            </div>
          </Tooltip>
        )}

        {/* Hover Overlay with Actions */}
        {hovered && (
          <Overlay opacity={0.1} color="#000">
            <Stack h="100%" justify="space-between" p="sm">
              <Group justify="flex-start">
                <AddToPlaylist video={video} />
              </Group>

              {onInfoClick && (
                <Group justify="flex-end">
                  <ActionIcon
                    variant="filled"
                    color="blue"
                    size="lg"
                    radius="xl"
                    onClick={onInfoButtonClick}
                    style={{
                      backgroundColor: 'rgba(34, 139, 230, 0.9)',
                      backdropFilter: 'blur(4px)'
                    }}>
                    <IconInfoCircle size={20} />
                  </ActionIcon>
                </Group>
              )}
            </Stack>
          </Overlay>
        )}
      </Card.Section>

      <Stack gap="xs" p="sm">
        <div>
          <Text size="sm" fw={600} lineClamp={2} title={video.title}>
            {video.title}
          </Text>
        </div>

        <Group justify="space-between" align="center">
          <Group gap="xs" align="center">
            <IconEye size={14} color="var(--mantine-color-dimmed)" />
            <Text size="xs" c="dimmed">
              {video.numberOfViews} views
            </Text>
          </Group>

          <Text size="xs" c="dimmed">
            {formattedDate}
          </Text>
        </Group>
      </Stack>
    </Card>
  );
};

export default VideoImageItem;
